import os
import pandas as pd
import numpy as np
from tensorflow.keras.models import Sequential, load_model
from tensorflow.keras.layers import Dense, Dropout, LSTM, Input
from sklearn.preprocessing import MinMaxScaler
from datetime import datetime
from joblib import dump, load
from class_files import Electric, Water, NaturalGas, Building
from database_connection import get_db_connection
from tensorflow.keras.callbacks import EarlyStopping, ReduceLROnPlateau
import logging
import shutil  # Yeni eklenen import

# Logger'ı yapılandırma
logging.basicConfig(
    filename='app.log',
    level=logging.INFO,
    format='%(asctime)s:%(levelname)s:%(message)s'
)

def feature_engineering(data, features_to_add):
    data = data.sort_index()
    for feature in features_to_add:
        if feature == 'year':
            data['year'] = data.index.year
        elif feature == 'month':
            data['month'] = data.index.month
        elif feature.startswith('lag_'):
            lag = int(feature.split('_')[1])
            data[feature] = data['Usage'].shift(lag)
        elif feature.startswith('diff_'):
            diff = int(feature.split('_')[1])
            data[feature] = data['Usage'].diff(diff)
        elif feature.startswith('rolling_mean_'):
            window = int(feature.split('_')[2])
            data[feature] = data['Usage'].rolling(window=window).mean()
    for col in data.columns:
        if 'lag' in col or 'diff' in col or 'rolling_mean' in col:
            data[col] = data[col].bfill().ffill()
        else:
            data[col] = data[col].ffill().bfill()
    return data

def create_dataset(data, target, look_back=5):
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
        self.file_base_path = os.path.join(
            "Consumptions",
            self.consumption_type.capitalize(),
            self.building_name,
            self.date_folder
        )
        os.makedirs(self.file_base_path, exist_ok=True)
        self.feature_scaler_filename = os.path.join(self.file_base_path, "feature_scaler.save")
        self.target_scaler_filename = os.path.join(self.file_base_path, "target_scaler.save")
        self.model_filename = os.path.join(self.file_base_path, "model.keras")
        self.prediction_csv = os.path.join(self.file_base_path, "predictions.csv")
        self.anomaly_csv = os.path.join(self.file_base_path, "anomalies.csv")

    def fetch_data(self):
        if self.consumption_type == "electric":
            return Electric.fetch_data(self.building_id)
        elif self.consumption_type == "water":
            return Water.fetch_data(self.building_id)
        elif self.consumption_type == "naturalgas":
            return NaturalGas.fetch_data(self.building_id)
        else:
            raise ValueError("Invalid consumption type. Supported types: electric, water, naturalgas")

    def prepare_data(self, df, fit_scaler=True):
        if df.empty:
            raise ValueError(f"No data found for {self.consumption_type} and building ID {self.building_id}")

        df['Date'] = pd.to_datetime(df['Date'])
        df['Year'] = df['Date'].dt.year
        df['Month'] = df['Date'].dt.month
        df_grouped = df.groupby(['Year', 'Month'], as_index=False).agg({'Usage': 'sum'})
        df_grouped['Date'] = pd.to_datetime(df_grouped[['Year', 'Month']].assign(Day=1))
        df_grouped.set_index('Date', inplace=True)
        df_grouped.drop(columns=['Year', 'Month'], inplace=True)

        features_to_add = [
            'year', 'month', 'lag_1', 'lag_3', 'lag_6',
            'diff_1', 'diff_3', 'rolling_mean_3', 'rolling_mean_6'
        ]
        df_grouped = feature_engineering(df_grouped, features_to_add)
        data = df_grouped.dropna()
        feature_cols = [col for col in data.columns if col != 'Usage']

        if isinstance(data['Usage'], list):
            data['Usage'] = np.array(data['Usage'])

        if fit_scaler:
            # Scaler'ları fit et ve kaydet
            self.feature_scaler = MinMaxScaler(feature_range=(0, 1))
            self.target_scaler = MinMaxScaler(feature_range=(0, 1))
            scaled_features = self.feature_scaler.fit_transform(data[feature_cols].values)
            scaled_target = self.target_scaler.fit_transform(data['Usage'].values.reshape(-1, 1))
            dump(self.feature_scaler, self.feature_scaler_filename)
            dump(self.target_scaler, self.target_scaler_filename)
            logging.info(f"Scalers fitted and saved to {self.feature_scaler_filename} and {self.target_scaler_filename}")
        else:
            # Scaler'ları yükle ve transform et
            self.feature_scaler = load(self.feature_scaler_filename)
            self.target_scaler = load(self.target_scaler_filename)
            scaled_features = self.feature_scaler.transform(data[feature_cols].values)
            scaled_target = self.target_scaler.transform(data['Usage'].values.reshape(-1, 1))
            logging.info(f"Scalers loaded from {self.feature_scaler_filename} and {self.target_scaler_filename}")

        return scaled_features, scaled_target, feature_cols

    def train_electric_model(self, scaled_features, scaled_target, feature_cols, look_back=5):
        X, Y = create_dataset(scaled_features, scaled_target, look_back)
        X = X.reshape((X.shape[0], X.shape[1], X.shape[2]))
        model = Sequential()
        model.add(Input(shape=(look_back, X.shape[2])))
        model.add(LSTM(50, activation='relu'))
        model.add(Dropout(0.2))
        model.add(Dense(1, activation='linear'))
        model.compile(optimizer='adam', loss='mse')
        callbacks = [
            EarlyStopping(monitor='loss', patience=10),
            ReduceLROnPlateau(monitor='loss', factor=0.2, patience=5, min_lr=0.0001)
        ]
        model.fit(X, Y, epochs=100, batch_size=16, callbacks=callbacks, verbose=0)
        model.save(self.model_filename)
        logging.info(f"Electric model saved to {self.model_filename}")

    def train_water_model(self, scaled_features, scaled_target, feature_cols, look_back=5):
        X, Y = create_dataset(scaled_features, scaled_target, look_back)
        X = X.reshape((X.shape[0], X.shape[1], X.shape[2]))
        model = Sequential()
        model.add(Input(shape=(look_back, X.shape[2])))
        model.add(LSTM(30, activation='relu'))
        model.add(Dropout(0.3))
        model.add(Dense(1, activation='linear'))
        model.compile(optimizer='adam', loss='mse')
        callbacks = [
            EarlyStopping(monitor='loss', patience=15),
            ReduceLROnPlateau(monitor='loss', factor=0.2, patience=7, min_lr=0.0001)
        ]
        model.fit(X, Y, epochs=150, batch_size=32, callbacks=callbacks, verbose=0)
        model.save(self.model_filename)
        logging.info(f"Water model saved to {self.model_filename}")

    def train_naturalgas_model(self, scaled_features, scaled_target, feature_cols, look_back=5):
        X, Y = create_dataset(scaled_features, scaled_target, look_back)
        X = X.reshape((X.shape[0], X.shape[1], X.shape[2]))
        model = Sequential()
        model.add(Input(shape=(look_back, X.shape[2])))
        model.add(LSTM(70, activation='relu'))
        model.add(Dropout(0.4))
        model.add(Dense(1, activation='linear'))
        model.compile(optimizer='adam', loss='mse')
        callbacks = [
            EarlyStopping(monitor='loss', patience=20),
            ReduceLROnPlateau(monitor='loss', factor=0.2, patience=10, min_lr=0.0001)
        ]
        model.fit(X, Y, epochs=200, batch_size=64, callbacks=callbacks, verbose=0)
        model.save(self.model_filename)
        logging.info(f"NaturalGas model saved to {self.model_filename}")

    def train_model(self, scaled_features, scaled_target, feature_cols, look_back=5, threshold=0.05):
        if self.consumption_type == "electric":
            self.train_electric_model(scaled_features, scaled_target, feature_cols, look_back)
        elif self.consumption_type == "water":
            self.train_water_model(scaled_features, scaled_target, feature_cols, look_back)
        elif self.consumption_type == "naturalgas":
            self.train_naturalgas_model(scaled_features, scaled_target, feature_cols, look_back)
        else:
            raise ValueError("Invalid consumption type for training.")
        
        # Tahminleri kaydet
        self.predict_and_save_csv(predict_months=12, look_back=look_back)
        
        # Anomali tespitini başlat
        self.detect_anomalies(threshold)

    def predict_and_save_csv(self, predict_months=12, look_back=5):
        if not os.path.exists(self.model_filename):
            raise FileNotFoundError(f"Model file for {self.consumption_type} not found")
        if not os.path.exists(self.feature_scaler_filename):
            raise FileNotFoundError(f"Feature scaler file for {self.consumption_type} not found")
        if not os.path.exists(self.target_scaler_filename):
            raise FileNotFoundError(f"Target scaler file for {self.consumption_type} not found")
        
        df = self.fetch_data()
        if df.empty:
            raise ValueError(f"No data available for prediction for {self.consumption_type}.")
        scaled_features, _, feature_cols = self.prepare_data(df, fit_scaler=False)
        x_input = scaled_features[-look_back:].reshape(1, look_back, len(feature_cols))
        trained_model = load_model(self.model_filename)
        predictions = []
        for _ in range(predict_months):
            pred = trained_model.predict(x_input, verbose=0)[0][0]
            predictions.append(pred)
            new_features = np.append(x_input[0, 1:, :], [[pred] + [0] * (len(feature_cols) - 1)], axis=0)
            x_input = new_features.reshape(1, look_back, len(feature_cols))
        
        # Ters ölçeklendirme
        inverse_predictions = self.target_scaler.inverse_transform(np.array(predictions).reshape(-1, 1)).flatten()
        
        if not isinstance(df.index, pd.DatetimeIndex):
            df['Date'] = pd.to_datetime(df['Date'])
            df.set_index('Date', inplace=True)
        df.sort_index(inplace=True)
        last_date = df.index[-1]
        future_dates = [last_date + pd.DateOffset(months=i + 1) for i in range(predict_months)]
        future_dates = [date.strftime('%Y-%m-%d %H:%M:%S+00:00') for date in future_dates]
        prediction_df = pd.DataFrame({'Date': future_dates, 'Predicted_Usage': inverse_predictions})
        prediction_df.to_csv(self.prediction_csv, index=False)
        logging.info(f"Predictions saved to {self.prediction_csv}")
        return prediction_df

    def get_predictions(self, months):
        if not os.path.exists(self.prediction_csv):
            raise FileNotFoundError(f"Prediction CSV for {self.consumption_type} not found")
        prediction_df = pd.read_csv(self.prediction_csv)
        return prediction_df.head(months)

    def detect_anomalies(self, threshold=0.05):
        try:
            logging.info("Anomaly detection started.")

            # Model ve target scaler dosyalarının varlığını kontrol et
            if not os.path.exists(self.model_filename):
                raise FileNotFoundError(f"Model file for {self.consumption_type} not found")
            if not os.path.exists(self.target_scaler_filename):
                raise FileNotFoundError(f"Target scaler file for {self.consumption_type} not found")

            logging.info("Model and target scaler files exist.")

            # Veriyi al ve hazırla (Scaler'ları yükle)
            df = self.fetch_data()
            if df.empty:
                raise ValueError(f"No data available for anomaly detection for {self.consumption_type}.")

            logging.info(f"Fetched data for anomaly detection. Data shape: {df.shape}")

            # Veriyi hazırla (Scaler'ları yükle)
            scaled_features, scaled_target, feature_cols = self.prepare_data(df, fit_scaler=False)
            logging.info(f"scaled_features shape: {scaled_features.shape}, scaled_target shape: {scaled_target.shape}")

            # Look-back ile veri setini oluştur
            look_back = 5  # Model eğitimi sırasında kullanılan look_back ile aynı olmalı
            X, Y = create_dataset(scaled_features, scaled_target, look_back=look_back)
            logging.info(f"X shape: {X.shape}, Y shape: {Y.shape}")

            # Model yükle
            trained_model = load_model(self.model_filename)
            logging.info(f"Trained model loaded from {self.model_filename}")

            # Tahmin yap
            predictions = trained_model.predict(X)
            logging.info(f"Predictions made. Predictions shape: {predictions.shape}, Predictions type: {type(predictions)}")

            # Hata hesapla
            error = np.abs(predictions.flatten() - Y.flatten())
            logging.info(f"Error calculated. Error shape: {error.shape}")

            # Anomalileri belirle
            anomalies = error > threshold
            logging.info(f"Anomalies detected. Total anomalies: {np.sum(anomalies)}")

            # Doğru tarih aralığını oluştur
            valid_dates = df.index[look_back:look_back + len(anomalies)]  # `anomalies` boyutuyla uyumlu indeksleme
            anomaly_dates = valid_dates[anomalies]
            logging.info(f"Anomaly dates extracted. Total anomaly dates: {len(anomaly_dates)}")

            # Eğer anomaliler varsa CSV'ye kaydet
            if len(anomaly_dates) > 0:
                anomaly_df = pd.DataFrame({
                    'Date': anomaly_dates.values,  # .values ile numpy array'e çevir
                    'Anomaly_Error': error[anomalies]
                })
                anomaly_df.to_csv(self.anomaly_csv, index=False)
                logging.info(f"Anomaly data saved to {self.anomaly_csv}")

                # Eski veri dizinlerini temizle
                self.cleanup_old_data()
                
                return anomaly_df
            else:
                logging.info("No anomalies detected.")
                # Eski veri dizinlerini temizle
                self.cleanup_old_data()
                return pd.DataFrame()
        except Exception as e:
            logging.error(f"Error during anomaly detection: {e}")
            raise

    def cleanup_old_data(self):
        """
        Bu metod, mevcut bina ve tüketim türü için daha eski olan tarih dizinlerini siler,
        sadece en güncel olanını korur.
        """
        try:
            logging.info("Cleanup of old data directories started.")

            # Parent directory: Consumptions/{ConsumptionType}/{BuildingName}/
            parent_dir = os.path.dirname(self.file_base_path)
            all_dirs = [d for d in os.listdir(parent_dir) if os.path.isdir(os.path.join(parent_dir, d))]

            date_dirs = []
            for d in all_dirs:
                try:
                    date = datetime.strptime(d, '%Y-%m-%d')
                    date_dirs.append((date, d))
                except ValueError:
                    # Tarih formatında olmayan dizinleri atla
                    pass

            # Tarihe göre sırala
            date_dirs.sort(key=lambda x: x[0])

            # En güncel dizini koru, diğerlerini sil
            if len(date_dirs) > 1:
                dirs_to_delete = [d for (date, d) in date_dirs[:-1]]
                for d in dirs_to_delete:
                    dir_path = os.path.join(parent_dir, d)
                    try:
                        shutil.rmtree(dir_path)
                        logging.info(f"Deleted old directory: {dir_path}")
                    except Exception as e:
                        logging.error(f"Failed to delete directory {dir_path}: {e}")
            else:
                logging.info("No old directories to delete.")

            logging.info("Cleanup of old data directories completed.")
        except Exception as e:
            logging.error(f"Error during cleanup of old data directories: {e}")
            raise
