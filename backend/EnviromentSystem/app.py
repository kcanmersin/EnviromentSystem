from flask import Flask, request, jsonify
import psycopg2
import pandas as pd
import numpy as np
from tensorflow.keras.models import Sequential, load_model
from tensorflow.keras.layers import Dense, Dropout
from sklearn.preprocessing import MinMaxScaler
import joblib
import os
from datetime import datetime

app = Flask(__name__)



def get_db_connection():
    conn = psycopg2.connect(**DB_PARAMS)
    return conn

class Water:
    @staticmethod
    def fetch_data(building_id=None):
        conn = get_db_connection()
        cursor = conn.cursor()
        query = 'SELECT "Date", "Usage" FROM "Waters"'  
        if building_id:
            query += f' WHERE "BuildingId" = %s'
        cursor.execute(query, (building_id,) if building_id else ())
        data = cursor.fetchall()
        conn.close()
        return pd.DataFrame(data, columns=['Date', 'Usage'])

class Electric:
    @staticmethod
    def fetch_data(building_id=None):
        conn = get_db_connection()
        cursor = conn.cursor()
        query = 'SELECT "Date", "Usage" FROM "Electrics"'  
        if building_id:
            query += f' WHERE "BuildingId" = %s'
        cursor.execute(query, (building_id,) if building_id else ())
        data = cursor.fetchall()
        conn.close()
        return pd.DataFrame(data, columns=['Date', 'Usage'])

class NaturalGas:
    @staticmethod
    def fetch_data(building_id=None):
        conn = get_db_connection()
        cursor = conn.cursor()
        query = 'SELECT "Date", "Usage" FROM "NaturalGasUsages"' 
        if building_id:
            query += f' WHERE "BuildingId" = %s'
        cursor.execute(query, (building_id,) if building_id else ())
        data = cursor.fetchall()
        conn.close()
        return pd.DataFrame(data, columns=['Date', 'Usage'])

class Building:
    @staticmethod
    def get_name(building_id):
        conn = get_db_connection()
        cursor = conn.cursor()
        cursor.execute('SELECT "Name" FROM "Buildings" WHERE "Id" = %s', (building_id,))
        result = cursor.fetchone()
        conn.close()
        return result[0] if result else "unknown"

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
        
        data = df['Usage'].values.reshape(-1, 1)
        self.scaler = MinMaxScaler(feature_range=(0, 1))
        scaled_data = self.scaler.fit_transform(data)
        joblib.dump(self.scaler, self.scaler_filename)
        return scaled_data

    def train_model(self, data, epochs=50, batch_size=16):
        model = Sequential([
            Dense(128, activation='relu', input_shape=(data.shape[1],)),
            Dropout(0.2),
            Dense(64, activation='relu'),
            Dropout(0.2),
            Dense(1, activation='linear')
        ])
        model.compile(optimizer='adam', loss='mean_squared_error')

        x_train = data[:-1] 
        y_train = data[1:] 
        model.fit(x_train, y_train, epochs=epochs, batch_size=batch_size, verbose=1)
        model.save(self.model_filename)

    def predict_and_save_csv(self, predict_months=12):
        if not os.path.exists(self.model_filename):
            raise FileNotFoundError(f"Model file for {self.consumption_type} not found")
        if not os.path.exists(self.scaler_filename):
            raise FileNotFoundError(f"Scaler file for {self.consumption_type} not found")

        df = self.fetch_data()
        if df.empty:
            raise ValueError(f"No data available for prediction for {self.consumption_type}.")

        data = self.prepare_data(df)
        x_input = data[-1] 
        trained_model = load_model(self.model_filename)
        predictions = []

        for _ in range(predict_months):
            pred = trained_model.predict(x_input.reshape(1, -1))[0][0]
            predictions.append(pred)
            x_input = np.append(x_input[1:], pred)

        scaler = joblib.load(self.scaler_filename)
        predictions = scaler.inverse_transform(np.array(predictions).reshape(-1, 1)).flatten().tolist()

        last_date = pd.to_datetime(df['Date'].iloc[-1])
        future_dates = [last_date + pd.DateOffset(months=i + 1) for i in range(predict_months)]

        prediction_df = pd.DataFrame({'Date': future_dates, 'Predicted_Usage': predictions})
        prediction_df.to_csv(self.prediction_csv, index=False)

        return prediction_df

    def detect_anomalies(self, threshold=0.05):
        if not os.path.exists(self.model_filename):
            raise FileNotFoundError(f"Model file for {self.consumption_type} not found")
        if not os.path.exists(self.scaler_filename):
            raise FileNotFoundError(f"Scaler file for {self.consumption_type} not found")

        df = self.fetch_data()
        if df.empty:
            raise ValueError(f"No data available for anomaly detection for {self.consumption_type}.")

        data = self.prepare_data(df)
        x_input = data

        trained_model = load_model(self.model_filename)
        scaler = joblib.load(self.scaler_filename)

        predictions = trained_model.predict(x_input)
        error = np.abs(predictions.flatten() - x_input.flatten())

        anomalies = error > threshold
        anomaly_dates = df['Date'][anomalies]

        if len(anomaly_dates) > 0:
            anomaly_df = pd.DataFrame({
                'Date': anomaly_dates,
                'Anomaly_Error': error[anomalies]
            })
            anomaly_df.to_csv(self.anomaly_csv, index=False)
            return anomaly_df
        else:
            return pd.DataFrame()

    def get_predictions(self, months):
        if not os.path.exists(self.prediction_csv):
            raise FileNotFoundError(f"Prediction CSV for {self.consumption_type} not found")
        prediction_df = pd.read_csv(self.prediction_csv)
        return prediction_df.head(months)

@app.route('/train_model', methods=['POST'])
def train_model():
    consumption_type = request.json.get('consumption_type', '').lower()
    building_id = request.json.get('building_id')
    epochs = request.json.get('epochs', 50)
    batch_size = request.json.get('batch_size', 16)
    threshold = float(request.args.get('threshold', 0.05))

    if not consumption_type:
        return jsonify({"error": "Consumption type (e.g., electric, water, naturalgas) is required"}), 400

    try:
        model = ConsumptionModel(consumption_type, building_id)
        df = model.fetch_data()
        if df.empty:
            return jsonify({"error": "No data available for the requested consumption type and building ID"}), 400
        data = model.prepare_data(df)
        model.train_model(data, epochs, batch_size)
        predictions_df = model.predict_and_save_csv()

        anomaly_df = model.detect_anomalies(threshold)

        response = {
            "message": f"Model for {consumption_type} trained and saved. Predictions for next 12 months saved to CSV.",
            "prediction_file": model.prediction_csv
        }

        if not anomaly_df.empty:
            response["anomaly_file"] = model.anomaly_csv

        return jsonify(response)
    except Exception as e:
        return jsonify({"error": str(e)}), 500


@app.route('/get_prediction', methods=['POST'])
def get_prediction():
    consumption_type = request.json.get('consumption_type', '').lower()
    building_id = request.json.get('building_id')
    months = request.json.get('months', 12)

    if not consumption_type:
        return jsonify({"error": "Consumption type (e.g., electric, water, naturalgas) is required"}), 400

    try:
        model = ConsumptionModel(consumption_type, building_id)
        predictions = model.get_predictions(months)
        return jsonify(predictions.to_dict(orient='records'))
    except Exception as e:
        return jsonify({"error": str(e)}), 500
@app.route('/get_anomaly', methods=['POST'])
def get_anomaly():
    consumption_type = request.json.get('consumption_type', '').lower()
    building_id = request.json.get('building_id')
    threshold = float(request.json.get('threshold', 0.05)) 

    if not consumption_type:
        return jsonify({"error": "Consumption type (e.g., electric, water, naturalgas) is required"}), 400

    try:
        model = ConsumptionModel(consumption_type, building_id)
        anomaly_df = model.detect_anomalies(threshold)
        if anomaly_df.empty:
            return jsonify({"message": "No anomalies detected."})

        return jsonify(anomaly_df.to_dict(orient='records'))
    except Exception as e:
        return jsonify({"error": str(e)}), 500


if __name__ == "__main__":
    app.run(debug=True)
