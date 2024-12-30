import os
import pandas as pd
import numpy as np
from tensorflow.keras.models import Sequential, load_model
from tensorflow.keras.layers import Dense, Dropout
from sklearn.preprocessing import MinMaxScaler
from datetime import datetime
from joblib import dump, load
from class_files import Electric, Water, NaturalGas, Building
from database_connection import get_db_connection
import joblib
from tensorflow.keras.layers import Conv1D, MaxPooling1D, Flatten
from tensorflow.keras.callbacks import EarlyStopping, ReduceLROnPlateau
from kerastuner.tuners import RandomSearch

def feature_engineering(data, features_to_add):
    data = data.sort_index()
    if 'year' in features_to_add:
        data['year'] = data.index.year
    if 'month' in features_to_add:
        data['month'] = data.index.month
    if 'lag_1' in features_to_add:
        data['lag_1'] = data['Usage'].shift(1)
    if 'lag_3' in features_to_add:
        data['lag_3'] = data['Usage'].shift(3)
    if 'lag_6' in features_to_add:
        data['lag_6'] = data['Usage'].shift(6)
    if 'diff_1' in features_to_add:
        data['diff_1'] = data['Usage'].diff(1)
    if 'diff_3' in features_to_add:
        data['diff_3'] = data['Usage'].diff(3)
    if 'rolling_mean_3' in features_to_add:
        data['rolling_mean_3'] = data['Usage'].rolling(window=3).mean()
    if 'rolling_mean_6' in features_to_add:
        data['rolling_mean_6'] = data['Usage'].rolling(window=6).mean()
    
    for col in data.columns:
        if 'lag' in col or 'diff' in col or 'rolling_mean' in col:
            data[col] = data[col].bfill().ffill()
        else:
            data[col] = data[col].ffill().bfill()
    return data
def create_dataset(data, target, look_back=12):
    X, Y = [], []
    for i in range(len(data) - look_back):
        X.append(data[i : i + look_back])
        Y.append(target[i + look_back])
    return np.array(X), np.array(Y)
class ConsumptionModel:
    def __init__(self, consumption_type, building_id=None):
        self.consumption_type = consumption_type.lower()
        self.building_id = building_id
        self.building_name = Building.get_name(building_id) if building_id else "all"
        self.date_folder = datetime.now().strftime('%Y-%m-%d')
        self.file_base_path = f"Consumptions/{self.consumption_type.capitalize()}/{self.building_name}/{self.date_folder}"
        os.makedirs(self.file_base_path, exist_ok=True)
        self.scaler_filename = f"{self.file_base_path}/scaler.save"
        self.model_filename = f"{self.file_base_path}/model.h5"
        self.prediction_csv = f"{self.file_base_path}/predictions.csv"
        self.anomaly_csv = f"{self.file_base_path}/anomalies.csv"


    def fetch_data(self):
        if self.consumption_type == "electric":
            return Electric.fetch_data(self.building_id)
        elif self.consumption_type == "water":
            return Water.fetch_data(self.building_id)
        elif self.consumption_type == "naturalgas":
            return NaturalGas.fetch_data(self.building_id)
        else:
            raise ValueError("Invalid consumption type. Supported types: electric, water, naturalgas")

    def prepare_data(self, df):
        if df.empty:
            raise ValueError(f"No data found for {self.consumption_type} and building ID {self.building_id}")

        df['Date'] = pd.to_datetime(df['Date'])
        df.set_index('Date', inplace=True)

        # Özellik mühendisliği işlemleri
        features_to_add = ['year', 'month', 'lag_1', 'lag_3', 'lag_6', 'diff_1', 'diff_3', 'rolling_mean_3', 'rolling_mean_6']
        df = feature_engineering(df, features_to_add)

        data = df.dropna()
        feature_cols = [col for col in data.columns if col != 'Usage']

        # `Usage` sütununun doğru türde olduğundan emin olun
        if isinstance(data['Usage'], list):
            data['Usage'] = np.array(data['Usage'])

        # Veriyi ölçeklendirme
        self.scaler = MinMaxScaler(feature_range=(0, 1))
        scaled_features = self.scaler.fit_transform(data[feature_cols].values)
        scaled_target = self.scaler.fit_transform(data['Usage'].values.reshape(-1, 1))

        # NumPy dizisine dönüştürme (Garanti)
        scaled_features = np.array(scaled_features)
        scaled_target = np.array(scaled_target)

        dump(self.scaler, self.scaler_filename)

        return scaled_features, scaled_target, feature_cols




    def train_model(self, scaled_features, scaled_target, feature_cols, look_back=12, test_size=24):
        # Eğitim ve test setlerini oluşturma
        train_size = len(scaled_target) - test_size
        scaled_train = scaled_features[:train_size]
        scaled_test = scaled_features[train_size:]
        target_train = scaled_target[:train_size]
        target_test = scaled_target[train_size:]

        # Dataset oluşturma
        def create_dataset(data, target, look_back=12):
            X, Y = [], []
            for i in range(len(data) - look_back):
                X.append(data[i : i + look_back])
                Y.append(target[i + look_back])
            return np.array(X), np.array(Y)

        X_train, y_train = create_dataset(scaled_train, target_train, look_back)
        X_test, y_test = create_dataset(scaled_test, target_test, look_back)

        # Modeli tanımlama
        model = Sequential([
            Conv1D(filters=64, kernel_size=2, activation='relu', input_shape=(X_train.shape[1], X_train.shape[2])),
            MaxPooling1D(pool_size=2),
            Flatten(),
            Dense(100, activation='relu'),
            Dropout(0.3),
            Dense(1)
        ])
        model.compile(optimizer='adam', loss='mean_squared_error')

        early_stopping = EarlyStopping(monitor='val_loss', patience=10, restore_best_weights=True)
        reduce_lr = ReduceLROnPlateau(monitor='val_loss', factor=0.2, patience=10, min_lr=1e-5)

        history = model.fit(
            X_train, y_train,
            epochs=50,
            batch_size=8,
            validation_data=(X_test, y_test),
            callbacks=[early_stopping, reduce_lr],
            verbose=1
        )

        # Modeli kaydetme
        model.save(self.model_filename)

        return history, X_test, y_test, model



    def predict_and_save_csv(self, predict_months=12):
        if not os.path.exists(self.model_filename):
            raise FileNotFoundError(f"Model file for {self.consumption_type} not found")
        if not os.path.exists(self.scaler_filename):
            raise FileNotFoundError(f"Scaler file for {self.consumption_type} not found")

        df = self.fetch_data()
        if df.empty:
            raise ValueError(f"No data available for prediction for {self.consumption_type}.")

        scaled_features, _, feature_cols = self.prepare_data(df)
        x_input = scaled_features[-12:].reshape(1, 12, len(feature_cols))  # (1, zaman_adımı, özellik_sayısı)

        print(f"x_input shape: {x_input.shape}")
        trained_model = load_model(self.model_filename)
        predictions = []

        for i in range(predict_months):
            # Tahmin yap
            pred = trained_model.predict(x_input)[0][0]
            print(f"Prediction {i + 1}: {pred}")
            predictions.append(pred)

            # Yeni giriş verisi oluştur (kaydırma)
            new_input = np.append(x_input[0, 1:, :], [[pred] + [0] * (len(feature_cols) - 1)], axis=0)
            x_input = new_input.reshape(1, 12, len(feature_cols))

        scaler = load(self.scaler_filename)
        predictions = scaler.inverse_transform(np.array(predictions).reshape(-1, 1)).flatten().tolist()

        last_date = pd.to_datetime(df.index[-1])
        future_dates = [last_date + pd.DateOffset(months=i + 1) for i in range(predict_months)]

        prediction_df = pd.DataFrame({'Date': future_dates, 'Predicted_Usage': predictions})
        print(f"Predictions DataFrame:\n{prediction_df}")
        prediction_df.to_csv(self.prediction_csv, index=False)

        return prediction_df
    def get_predictions(self, months):
        if not os.path.exists(self.prediction_csv):
            raise FileNotFoundError(f"Prediction CSV for {self.consumption_type} not found")
        prediction_df = pd.read_csv(self.prediction_csv)
        return prediction_df.head(months)

    def detect_anomalies(self, threshold=0.05):
        try:
            print("Anomaly detection started.")

            # Model ve scaler dosyalarının varlığını kontrol et
            if not os.path.exists(self.model_filename):
                raise FileNotFoundError(f"Model file for {self.consumption_type} not found")
            if not os.path.exists(self.scaler_filename):
                raise FileNotFoundError(f"Scaler file for {self.consumption_type} not found")

            print("Model and scaler files exist.")

            # Veriyi al ve hazırla
            df = self.fetch_data()
            if df.empty:
                raise ValueError(f"No data available for anomaly detection for {self.consumption_type}.")

            print(f"Fetched data for anomaly detection. Data shape: {df.shape}")

            # Veriyi hazırla
            scaled_features, scaled_target, feature_cols = self.prepare_data(df)
            print(f"scaled_features shape: {scaled_features.shape}, scaled_target shape: {scaled_target.shape}")

            # Look-back ile veri setini oluştur
            look_back = 12  # Model eğitimi sırasında kullanılan look_back ile aynı olmalı
            X, Y = create_dataset(scaled_features, scaled_target, look_back=look_back)

            print(f"X shape: {X.shape}, Y shape: {Y.shape}")

            # Model ve scaler yükle
            trained_model = load_model(self.model_filename)
            print(f"Trained model loaded from {self.model_filename}")

            scaler = joblib.load(self.scaler_filename)
            print(f"Scaler loaded from {self.scaler_filename}")

            # Tahmin yap
            predictions = trained_model.predict(X)
            print(f"Predictions made. Predictions shape: {predictions.shape}, Predictions type: {type(predictions)}")

            # Hata hesapla
            error = np.abs(predictions.flatten() - Y.flatten())
            print(f"Error calculated. Error shape: {error.shape}")

            # Anomalileri belirle
            anomalies = error > threshold
            print(f"Anomalies detected. Total anomalies: {np.sum(anomalies)}")

            # Anomali tarihlerini çıkar
            anomaly_dates = df.index[look_back:][anomalies]
            print(f"Anomaly dates extracted. Total anomaly dates: {len(anomaly_dates)}")

            # Eğer anomaliler varsa CSV'ye kaydet
            if len(anomaly_dates) > 0:
                anomaly_df = pd.DataFrame({
                    'Date': anomaly_dates,
                    'Anomaly_Error': error[anomalies]
                })
                anomaly_df.to_csv(self.anomaly_csv, index=False)
                print(f"Anomaly data saved to {self.anomaly_csv}")
                return anomaly_df
            else:
                print("No anomalies detected.")
                return pd.DataFrame()
        except Exception as e:
            print(f"Error during anomaly detection: {e}")
            raise

