import decimal
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
        if ('lag' in col or 'diff' in col or 'rolling_mean' in col or
                'rolling_std' in col or 'rolling_max' in col or 'rolling_min' in col):
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

        # "temp" ve "final" klasörleri için yollar
        self.base_path = os.path.join("Consumptions", self.consumption_type.capitalize(), self.building_name)
        self.temp_base_path = os.path.join(self.base_path, "temp")
        self.final_base_path = os.path.join(self.base_path, "final")
        
        # Klasörleri oluştur
        os.makedirs(self.temp_base_path, exist_ok=True)
        os.makedirs(self.final_base_path, exist_ok=True)

        # Dosya yolları (varsayılan olarak temp'e yaz)
        self.feature_scaler_filename = os.path.join(self.temp_base_path, "feature_scaler.save")
        self.target_scaler_filename = os.path.join(self.temp_base_path, "target_scaler.save")
        self.model_filename = os.path.join(self.temp_base_path, "model.keras")
        self.prediction_csv = os.path.join(self.temp_base_path, "predictions.csv")
        self.anomaly_csv = os.path.join(self.temp_base_path, "anomalies.csv")

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
        features_to_add = [
            'year', 'month', 'lag_1', 'lag_3', 'lag_6',
            'diff_1', 'diff_3', 'rolling_mean_3', 'rolling_mean_6',
            'month_sin_cos', 'z_score'
        ]
        df_grouped = feature_engineering(df_grouped, features_to_add)
        data = df_grouped.dropna()

        logging.info(f"Data ID: {self.building_id}")
        logging.info(f"Data head: {data.head()}")

        feature_cols = [col for col in data.columns if col != 'Usage']

        if fit_scaler:
            # Ölçekleyicileri fit edip kaydet (temp klasörüne)
            self.feature_scaler = MinMaxScaler(feature_range=(0, 1))
            self.target_scaler = MinMaxScaler(feature_range=(0, 1))

            scaled_features = self.feature_scaler.fit_transform(data[feature_cols].values)
            scaled_target = self.target_scaler.fit_transform(data['Usage'].values.reshape(-1, 1))

            dump(self.feature_scaler, self.feature_scaler_filename)
            dump(self.target_scaler, self.target_scaler_filename)

            logging.info(f"Scalers fitted and saved to {self.feature_scaler_filename} and {self.target_scaler_filename}")
        else:
            # Eğer tekrar tahmin/anomali için kullanacaksak final klasöründeki scaler'ı kullanmaya çalışıyoruz
            final_feature_scaler_path = os.path.join(self.final_base_path, "feature_scaler.save")
            final_target_scaler_path = os.path.join(self.final_base_path, "target_scaler.save")

            if os.path.exists(final_feature_scaler_path) and os.path.exists(final_target_scaler_path):
                # final'daki scaler'lar yüklensin
                self.feature_scaler = load(final_feature_scaler_path)
                self.target_scaler = load(final_target_scaler_path)
                logging.info("Scalers loaded from final folder.")
            else:
                # Aksi halde temp'deki en güncel scaler'ları yükle
                self.feature_scaler = load(self.feature_scaler_filename)
                self.target_scaler = load(self.target_scaler_filename)
                logging.info("Scalers loaded from temp folder.")

            scaled_features = self.feature_scaler.transform(data[feature_cols].values)
            scaled_target = self.target_scaler.transform(data['Usage'].values.reshape(-1, 1))

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

        # Modeli temp klasörüne kaydediyoruz
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
        
        # Eğitim sonrası tahmin yap
        self.predict_and_save_csv(predict_months=12, look_back=look_back)
        
        # Anomali tespiti yap
        self.detect_anomalies(threshold)

        # Tüm dosyaları temp'ten final'e taşı
        self.move_temp_to_final()

    def move_temp_to_final(self):
        """
        Eğitim bittikten sonra temp klasöründeki güncel model, scaler ve CSV dosyalarını final klasörüne taşır.
        """
        for filename in os.listdir(self.temp_base_path):
            src = os.path.join(self.temp_base_path, filename)
            dst = os.path.join(self.final_base_path, filename)
            try:
                shutil.move(src, dst)
                logging.info(f"Moved {filename} from temp to final folder.")
            except Exception as e:
                logging.error(f"Failed to move {filename} to final folder: {e}")

    def predict_and_save_csv(self, predict_months=12, look_back=5):
        """
        Gelecek aylar için tüketim tahminleri yapar ve bunları CSV dosyasına (temp'e) kaydeder.
        """
        # final veya temp'teki model dosyalarına bakılarak yüklenir
        model_path = os.path.join(self.final_base_path, "model.keras")
        if not os.path.exists(model_path):
            model_path = self.model_filename  # temp'teki model

        if not os.path.exists(model_path):
            raise FileNotFoundError(f"Model file for {self.consumption_type} not found in temp or final folder")

        # Aynı mantıkla scaler'ları da final veya temp'ten yükleyelim
        feature_scaler_path = os.path.join(self.final_base_path, "feature_scaler.save")
        target_scaler_path = os.path.join(self.final_base_path, "target_scaler.save")

        if not os.path.exists(feature_scaler_path):
            feature_scaler_path = self.feature_scaler_filename
        if not os.path.exists(target_scaler_path):
            target_scaler_path = self.target_scaler_filename

        df = self.fetch_data()
        if df.empty:
            raise ValueError(f"No data available for prediction for {self.consumption_type}.")

        # Model ve scaler objelerini yükle
        trained_model = load_model(model_path)
        loaded_feature_scaler = load(feature_scaler_path)
        loaded_target_scaler = load(target_scaler_path)

        # Veri hazırla (fit_scaler=False; tekrar ölçekleyiciler fit edilmesin)
        scaled_features, _, feature_cols = self.prepare_data(df, fit_scaler=False)

        x_input = scaled_features[-look_back:].reshape(1, look_back, len(feature_cols))
        predictions = []

        for _ in range(predict_months):
            pred = trained_model.predict(x_input, verbose=0)[0][0]
            predictions.append(pred)
            # Yeni tahmini, girdi dizisine ekle
            new_features = np.append(
                x_input[0, 1:, :], 
                [[pred] + [0]*(len(feature_cols)-1)],
                axis=0
            )
            x_input = new_features.reshape(1, look_back, len(feature_cols))

        # Ters dönüşüm
        inverse_predictions = loaded_target_scaler.inverse_transform(
            np.array(predictions).reshape(-1, 1)
        ).flatten()

        if not isinstance(df.index, pd.DatetimeIndex):
            df['Date'] = pd.to_datetime(df['Date'])
            df.set_index('Date', inplace=True)
        df.sort_index(inplace=True)
        last_date = df.index[-1]

        # Gelecek ayların tarihlerini oluştur
        future_dates_raw = [last_date + pd.DateOffset(months=i + 1) for i in range(predict_months)]
        future_dates = []
        for date in future_dates_raw:
            ts = pd.Timestamp(date)
            if ts.tzinfo is None:
                ts = ts.tz_localize('UTC')
            else:
                ts = ts.tz_convert('UTC')
            future_dates.append(ts.strftime('%Y-%m-%d %H:%M:%S%z'))

        prediction_df = pd.DataFrame({'Date': future_dates, 'Predicted_Usage': inverse_predictions})
        prediction_df.to_csv(self.prediction_csv, index=False)
        logging.info(f"Predictions saved to {self.prediction_csv}")
        return prediction_df

    def detect_anomalies(self, threshold=0.05):
        """
        Tahmin hatalarına dayanarak anomalileri tespit eder ve önce temp'e kaydeder.
        Ardından temp'deki anomalies.csv dosyasını final klasörüne taşır (eskisini silerek).
        """
        logging.info("Anomaly detection started.")

        # Model yolu (önce final'de arıyoruz, yoksa temp'teki)
        model_path = os.path.join(self.final_base_path, "model.keras")
        if not os.path.exists(model_path):
            model_path = self.model_filename  # temp'teki model

        if not os.path.exists(model_path):
            raise FileNotFoundError(f"Model file for {self.consumption_type} not found in temp or final folder")

        # Target scaler yolu (önce final'de arıyoruz, yoksa temp'teki)
        target_scaler_path = os.path.join(self.final_base_path, "target_scaler.save")
        if not os.path.exists(target_scaler_path):
            target_scaler_path = self.target_scaler_filename

        df = self.fetch_data()
        if df.empty:
            raise ValueError(f"No data available for anomaly detection for {self.consumption_type}.")

        # Ölçeklenmiş verileri tekrar yükle (fit_scaler=False)
        scaled_features, scaled_target, feature_cols = self.prepare_data(df, fit_scaler=False)

        look_back = 5
        X, Y = create_dataset(scaled_features, scaled_target, look_back=look_back)

        trained_model = load_model(model_path)
        predictions = trained_model.predict(X)
        error = np.abs(predictions.flatten() - Y.flatten())

        # threshold üstü hataları anomali kabul et
        anomalies = error > threshold
        valid_indices = np.arange(look_back, look_back + len(anomalies))
        anomaly_indices = valid_indices[anomalies]

        if len(anomaly_indices) > 0:
            anomaly_df = pd.DataFrame({
                'Date': anomaly_indices,
                'Anomaly_Error': error[anomalies]
            })
            # 1) Anomalileri temp klasörüne kaydet
            anomaly_df.to_csv(self.anomaly_csv, index=False)
            logging.info(f"Anomaly data saved to {self.anomaly_csv}")

            # 2) Ardından anomalies.csv'yi temp'ten final'e taşı (varsa eskisini silerek)
            self._move_anomalies_temp_to_final()
            return anomaly_df
        else:
            logging.info("No anomalies detected.")
            return pd.DataFrame()

    def _move_anomalies_temp_to_final(self):
        """
        temp klasöründeki anomalies.csv dosyasını final klasörüne taşır.
        Eğer final klasöründe anomalies.csv varsa, önce onu siler.
        """
        temp_anomaly_path = self.anomaly_csv  # temp/…/anomalies.csv
        final_anomaly_path = os.path.join(self.final_base_path, "anomalies.csv")

        # Eğer final klasöründe anomalies.csv varsa sil
        if os.path.exists(final_anomaly_path):
            os.remove(final_anomaly_path)

        # Eğer temp'teki anomalies.csv varsa taşı
        if os.path.exists(temp_anomaly_path):
            shutil.move(temp_anomaly_path, final_anomaly_path)
            logging.info(f"Moved anomalies.csv from {temp_anomaly_path} to {final_anomaly_path}")
        else:
            logging.warning(f"No anomalies.csv found in temp folder to move.")

    def get_predictions(self, months=12):
        prediction_csv_path = os.path.join(self.final_base_path, "predictions.csv")
        if os.path.exists(prediction_csv_path):
            logging.info(f"Prediction CSV found in final folder: {prediction_csv_path}")
            prediction_df = pd.read_csv(prediction_csv_path)

            # İstenen sayıda tahmini (ilk `months` satır) döndür
            return prediction_df.head(months)
        else:
            logging.warning(f"No prediction CSV found in final folder: {self.final_base_path}")
            return pd.DataFrame()  # Boş DataFrame döner
