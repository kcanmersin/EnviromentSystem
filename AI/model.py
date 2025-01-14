import os
import pandas as pd
import numpy as np
import random
import logging
import shutil
from datetime import datetime
from joblib import dump, load
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras.models import Sequential, load_model
from tensorflow.keras.layers import Dense, Dropout, Conv1D, MaxPooling1D, Flatten, Input
from tensorflow.keras.callbacks import EarlyStopping, ReduceLROnPlateau
from class_files import Electric, Water, NaturalGas, Building
from database_connection import get_db_connection
import tensorflow as tf

# Rastgelelik için tohum ayarlamaları (Opsiyonel, tekrarlanabilirlik için)
SEED = 42
np.random.seed(SEED)
tf.random.set_seed(SEED)
random.seed(SEED)
os.environ['PYTHONHASHSEED'] = str(SEED)

# Logger Yapılandırması
logging.basicConfig(
    filename='app.log',
    level=logging.INFO,
    format='%(asctime)s:%(levelname)s:%(message)s'
)

def feature_engineering(data, features_to_add):
    data = data.sort_index()

    # Ensure 'Usage' is float
    if data['Usage'].dtype == 'object' or isinstance(data['Usage'].iloc[0], decimal.Decimal):
        data['Usage'] = data['Usage'].astype(float)

    # Basic features
    if 'year' in features_to_add:
        data['year'] = data.index.year

    if 'month' in features_to_add:
        data['month'] = data.index.month

    # Time-series features
    if 'lag_1' in features_to_add:
        data['lag_1'] = data['Usage'].shift(1)
    if 'lag_3' in features_to_add:
        data['lag_3'] = data['Usage'].shift(3)
    if 'lag_6' in features_to_add:
        data['lag_6'] = data['Usage'].shift(6)

    # Difference features
    if 'diff_1' in features_to_add:
        data['diff_1'] = data['Usage'].diff(1)
    if 'diff_3' in features_to_add:
        data['diff_3'] = data['Usage'].diff(3)

    # Rolling mean
    if 'rolling_mean_3' in features_to_add:
        data['rolling_mean_3'] = data['Usage'].rolling(window=3).mean()
    if 'rolling_mean_6' in features_to_add:
        data['rolling_mean_6'] = data['Usage'].rolling(window=6).mean()

    # Rolling standard deviation
    if 'rolling_std_3' in features_to_add:
        data['rolling_std_3'] = data['Usage'].rolling(window=3).std()
    if 'rolling_std_6' in features_to_add:
        data['rolling_std_6'] = data['Usage'].rolling(window=6).std()

    # Rolling max and min
    if 'rolling_max_3' in features_to_add:
        data['rolling_max_3'] = data['Usage'].rolling(window=3).max()
    if 'rolling_min_3' in features_to_add:
        data['rolling_min_3'] = data['Usage'].rolling(window=3).min()

    # Z-score normalization
    if 'z_score' in features_to_add:
        data['z_score'] = (data['Usage'] - data['Usage'].mean()) / data['Usage'].std()

    # Seasonal components
    if 'is_winter' in features_to_add:
        data['is_winter'] = data.index.month.isin([12, 1, 2]).astype(int)
    if 'is_summer' in features_to_add:
        data['is_summer'] = data.index.month.isin([6, 7, 8]).astype(int)
    if 'is_spring' in features_to_add:
        data['is_spring'] = data.index.month.isin([3, 4, 5]).astype(int)
    if 'is_autumn' in features_to_add:
        data['is_autumn'] = data.index.month.isin([9, 10, 11]).astype(int)

    # Trigonometric features for months
    if 'month_sin_cos' in features_to_add:
        months = data.index.month
        data['month_sin'] = np.sin(2 * np.pi * months / 12)
        data['month_cos'] = np.cos(2 * np.pi * months / 12)

    # Growth rate
    if 'growth_rate' in features_to_add:
        data['growth_rate'] = data['Usage'].pct_change()

    # Fill missing values for new columns
    for col in data.columns:
        if 'lag' in col or 'diff' in col or 'rolling_mean' in col or 'rolling_std' in col or 'rolling_max' in col or 'rolling_min' in col:
            data[col] = data[col].fillna(method='bfill').fillna(method='ffill')
        elif col == 'growth_rate' or col == 'z_score':
            data[col] = data[col].fillna(0)
        else:
            data[col] = data[col].fillna(method='ffill').fillna(method='bfill')

    return data


def create_dataset(data, target, look_back=5):
    """
    Denetimli öğrenme için veri seti oluşturur.
    """
    X, Y = [], []
    for i in range(len(data) - look_back):
        X.append(data[i : i + look_back])
        Y.append(target[i + look_back])
    return np.array(X), np.array(Y)

class ConsumptionModel:
    def __init__(self, consumption_type, building_id=None):
        """
        ConsumptionModel sınıfını belirli bir tüketim türü ve bina ID'si ile başlatır.
        """
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
        """
        Tüketim türüne ve bina ID'sine göre veriyi çeker.
        """
        if self.consumption_type == "electric":
            return Electric.fetch_data(self.building_id)
        elif self.consumption_type == "water":
            return Water.fetch_data(self.building_id)
        elif self.consumption_type == "naturalgas":
            return NaturalGas.fetch_data(self.building_id)
        else:
            raise ValueError("Invalid consumption type. Supported types: electric, water, naturalgas")

    def prepare_data(self, df, fit_scaler=True):
        """
        Modelleme için veriyi hazırlar ve ölçeklendirir.
        """
        if df.empty:
            raise ValueError(f"No data found for {self.consumption_type} and building ID {self.building_id}")

        df['Date'] = pd.to_datetime(df['Date'])
        df['Year'] = df['Date'].dt.year
        df['Month'] = df['Date'].dt.month
        df_grouped = df.groupby(['Year', 'Month'], as_index=False).agg({'Usage': 'sum'})
        df_grouped['Date'] = pd.to_datetime(df_grouped[['Year', 'Month']].assign(Day=1))
        df_grouped.set_index('Date', inplace=True)
        df_grouped.drop(columns=['Year', 'Month'], inplace=True)

        # Ek özellikler tanımlanıyor
        # features_to_add = [
        #     'year', 'month', 'lag_1', 'lag_3', 'lag_6'

        # ]
        features_to_add = [
            'year', 'month', 'lag_1', 'lag_3', 'lag_6',
            'diff_1', 'diff_3', 'rolling_mean_3', 'rolling_mean_6','month_sin_cos','z_score'
        ]
        df_grouped = feature_engineering(df_grouped, features_to_add)
        data = df_grouped.dropna()
        #log data  head and id 
        logging.info(f"Data ID: {self.building_id}")
        logging.info(f"Data head: {data.head()}")
        
        feature_cols = [col for col in data.columns if col != 'Usage']

        if isinstance(data['Usage'], list):
            data['Usage'] = np.array(data['Usage'])

        if fit_scaler:
            # Ölçekleyicileri fit edip kaydet
            self.feature_scaler = MinMaxScaler(feature_range=(0, 1))
            self.target_scaler = MinMaxScaler(feature_range=(0, 1))
            scaled_features = self.feature_scaler.fit_transform(data[feature_cols].values)
            scaled_target = self.target_scaler.fit_transform(data['Usage'].values.reshape(-1, 1))
            dump(self.feature_scaler, self.feature_scaler_filename)
            dump(self.target_scaler, self.target_scaler_filename)
            logging.info(f"Scalers fitted and saved to {self.feature_scaler_filename} and {self.target_scaler_filename}")
        else:
            # Ölçekleyicileri yükleyip dönüştür
            self.feature_scaler = load(self.feature_scaler_filename)
            self.target_scaler = load(self.target_scaler_filename)
            scaled_features = self.feature_scaler.transform(data[feature_cols].values)
            scaled_target = self.target_scaler.transform(data['Usage'].values.reshape(-1, 1))
            logging.info(f"Scalers loaded from {self.feature_scaler_filename} and {self.target_scaler_filename}")

        return scaled_features, scaled_target, feature_cols

    def train_electric_model(self, scaled_features, scaled_target, feature_cols, look_back=5):
        """
        Elektrik tüketimi için CNN modeli eğitir.
        """
        X, Y = create_dataset(scaled_features, scaled_target, look_back)
        model = Sequential()
        model.add(Input(shape=(look_back, X.shape[2])))
        model.add(Conv1D(filters=64, kernel_size=2, activation='relu'))
        model.add(MaxPooling1D(pool_size=2))
        model.add(Flatten())
        model.add(Dense(50, activation='relu'))
        model.add(Dropout(0.2))
        model.add(Dense(1, activation='softplus'))

        model.compile(optimizer='adam', loss='mse')
        callbacks = [
            EarlyStopping(monitor='loss', patience=10, restore_best_weights=True),
            ReduceLROnPlateau(monitor='loss', factor=0.2, patience=5, min_lr=0.0001)
        ]
        model.fit(X, Y, epochs=100, batch_size=16, callbacks=callbacks, verbose=1)

        model.save(self.model_filename)
        logging.info(f"Electric model saved to {self.model_filename}")



    def train_water_model(self, scaled_features, scaled_target, feature_cols, look_back=5):
        X, Y = create_dataset(scaled_features, scaled_target, look_back)

        model = Sequential()
        model.add(Input(shape=(look_back, X.shape[2])))
        model.add(Conv1D(filters=32, kernel_size=2, activation='relu'))
        model.add(MaxPooling1D(pool_size=2))
        model.add(Flatten())
        model.add(Dense(30, activation='relu'))
        model.add(Dropout(0.3))
        model.add(Dense(1, activation='softplus'))

        model.compile(optimizer='adam', loss='mse')
        callbacks = [
                EarlyStopping(monitor='loss', patience=15, restore_best_weights=True),
                ReduceLROnPlateau(monitor='loss', factor=0.2, patience=7, min_lr=0.0001)
            ]
        model.fit(X, Y, epochs=150, batch_size=32, callbacks=callbacks, verbose=1)

        model.save(self.model_filename)
        logging.info(f"Water model saved to {self.model_filename}")

    def train_naturalgas_model(self, scaled_features, scaled_target, feature_cols, look_back=5):
        X, Y = create_dataset(scaled_features, scaled_target, look_back)

        model = Sequential()
        model.add(Input(shape=(look_back, X.shape[2])))
        model.add(Conv1D(filters=128, kernel_size=2, activation='relu'))
        model.add(MaxPooling1D(pool_size=2))
        model.add(Flatten())
        model.add(Dense(70, activation='relu'))
        model.add(Dropout(0.4))
        model.add(Dense(1, activation='softplus'))

        model.compile(optimizer='adam', loss='mse')
        callbacks = [
            EarlyStopping(monitor='loss', patience=20, restore_best_weights=True),
            ReduceLROnPlateau(monitor='loss', factor=0.2, patience=10, min_lr=0.0001)
        ]
        model.fit(X, Y, epochs=200, batch_size=64, callbacks=callbacks, verbose=1)

        model.save(self.model_filename)
        logging.info(f"NaturalGas model saved to {self.model_filename}")

    def train_model(self, scaled_features, scaled_target, feature_cols, look_back=5, threshold=0.05):
        """
        Belirtilen tüketim türüne göre uygun modeli eğitir.
        """
        if self.consumption_type == "electric":
            self.train_electric_model(scaled_features, scaled_target, feature_cols, look_back)
        elif self.consumption_type == "water":
            self.train_water_model(scaled_features, scaled_target, feature_cols, look_back)
        elif self.consumption_type == "naturalgas":
            self.train_naturalgas_model(scaled_features, scaled_target, feature_cols, look_back)
        else:
            raise ValueError("Invalid consumption type for training.")
        
        self.predict_and_save_csv(predict_months=12, look_back=look_back)
        
        self.detect_anomalies(threshold)

    def predict_and_save_csv(self, predict_months=12, look_back=5):
        """
        Gelecek aylar için tüketim tahminleri yapar ve bunları CSV dosyasına kaydeder.
        """
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
            # Update input by shifting and adding the new prediction
            new_features = np.append(x_input[0, 1:, :], [[pred] + [0]*(len(feature_cols)-1)], axis=0)
            x_input = new_features.reshape(1, look_back, len(feature_cols))

        inverse_predictions = self.target_scaler.inverse_transform(np.array(predictions).reshape(-1, 1)).flatten()

        if not isinstance(df.index, pd.DatetimeIndex):
            df['Date'] = pd.to_datetime(df['Date'])
            df.set_index('Date', inplace=True)
        df.sort_index(inplace=True)
        last_date = df.index[-1]

        # Generate future dates
        future_dates_raw = [last_date + pd.DateOffset(months=i + 1) for i in range(predict_months)]
        future_dates = []
        for date in future_dates_raw:
            ts = pd.Timestamp(date)
            if ts.tzinfo is None:
                # If naive, localize to UTC
                ts = ts.tz_localize('UTC')
            else:
                # If aware, convert to UTC
                ts = ts.tz_convert('UTC')
            future_dates.append(ts.strftime('%Y-%m-%d %H:%M:%S%z'))

        prediction_df = pd.DataFrame({'Date': future_dates, 'Predicted_Usage': inverse_predictions})
        prediction_df.to_csv(self.prediction_csv, index=False)
        logging.info(f"Predictions saved to {self.prediction_csv}")
        return prediction_df
    def get_predictions(self, months):
        """
        Tahmin CSV dosyasından belirtilen sayıda tahmini döndürür.
        """
        if not os.path.exists(self.prediction_csv):
            raise FileNotFoundError(f"Prediction CSV for {self.consumption_type} not found")
        prediction_df = pd.read_csv(self.prediction_csv)
        return prediction_df.head(months)

    def detect_anomalies(self, threshold=0.05):
        """
        Tahmin hatalarına dayanarak anomalileri tespit eder ve kaydeder.
        """
        try:
            logging.info("Anomaly detection started.")

            if not os.path.exists(self.model_filename):
                raise FileNotFoundError(f"Model file for {self.consumption_type} not found")
            if not os.path.exists(self.target_scaler_filename):
                raise FileNotFoundError(f"Target scaler file for {self.consumption_type} not found")

            logging.info("Model and target scaler files exist.")

            df = self.fetch_data()
            if df.empty:
                raise ValueError(f"No data available for anomaly detection for {self.consumption_type}.")

            logging.info(f"Fetched data for anomaly detection. Data shape: {df.shape}")

            scaled_features, scaled_target, feature_cols = self.prepare_data(df, fit_scaler=False)
            logging.info(f"scaled_features shape: {scaled_features.shape}, scaled_target shape: {scaled_target.shape}")

            look_back = 5  
            X, Y = create_dataset(scaled_features, scaled_target, look_back=look_back)
            logging.info(f"X shape: {X.shape}, Y shape: {Y.shape}")

            trained_model = load_model(self.model_filename)
            logging.info(f"Trained model loaded from {self.model_filename}")

            predictions = trained_model.predict(X)
            logging.info(f"Predictions made. Predictions shape: {predictions.shape}, Predictions type: {type(predictions)}")

            # Hata hesapla
            error = np.abs(predictions.flatten() - Y.flatten())
            logging.info(f"Error calculated. Error shape: {error.shape}")

            # Anomalileri belirle
            anomalies = error > threshold
            logging.info(f"Anomalies detected. Total anomalies: {np.sum(anomalies)}")

            # Doğru indeks aralığını oluştur
            # Anomalilerin bulunduğu veri kümesindeki orijinal indeksleri alıyoruz
            valid_indices = np.arange(look_back, look_back + len(anomalies))
            anomaly_indices = valid_indices[anomalies]
            logging.info(f"Anomaly indices extracted. Total anomaly indices: {len(anomaly_indices)}")

            # Map anomaly indices to actual dates
            anomaly_dates = pd.to_datetime(df.iloc[anomaly_indices]['Date']).dt.strftime('%Y-%m-%d %H:%M:%S+00:00')

            # Eğer anomaliler varsa CSV'ye kaydet
            if len(anomaly_indices) > 0:
                # Anomaly indices'i 'Date' sütunu olarak kullanıyoruz
                anomaly_df = pd.DataFrame({
                    'Date': anomaly_dates,
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
        Eski tarih dizinlerini siler, sadece en güncelini korur.
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
