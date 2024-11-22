from flask import Flask, request, jsonify
from flask_sqlalchemy import SQLAlchemy
from flask_cors import CORS
import pandas as pd
import numpy as np
from tensorflow.keras.models import Sequential, load_model
from tensorflow.keras.layers import Dense, Dropout
from sklearn.preprocessing import MinMaxScaler
import joblib
import os
from datetime import datetime

app = Flask(__name__)
CORS(app)

app.config['SQLALCHEMY_DATABASE_URI'] = "sqlite:///consumptions.db"
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
db = SQLAlchemy(app)

class Water(db.Model):
    __tablename__ = "Waters"
    id = db.Column("Id", db.String, primary_key=True)
    date = db.Column("Date", db.Date, nullable=False)
    usage = db.Column("Usage", db.Float, nullable=False)

class Electric(db.Model):
    __tablename__ = "Electrics"
    id = db.Column("Id", db.String, primary_key=True)
    date = db.Column("Date", db.Date, nullable=False)
    usage = db.Column("Usage", db.Float, nullable=False)
    building_id = db.Column("BuildingId", db.String)

class NaturalGas(db.Model):
    __tablename__ = "NaturalGasUsages"
    id = db.Column("Id", db.String, primary_key=True)
    date = db.Column("Date", db.Date, nullable=False)
    usage = db.Column("Usage", db.Float, nullable=False)
    building_id = db.Column("BuildingId", db.String)

class Building(db.Model):
    __tablename__ = "Buildings"
    id = db.Column("Id", db.String, primary_key=True)
    name = db.Column("Name", db.String, nullable=False)

class ConsumptionModel:
    def __init__(self, consumption_type, building_id=None):
        self.consumption_type = consumption_type.lower()
        self.building_id = building_id
        self.building_name = self.get_building_name(building_id) if building_id else "all"
        self.date_folder = datetime.now().strftime('%Y-%m-%d')
        self.file_base_path = f"Consumptions/{self.consumption_type.capitalize()}/{self.building_name}/{self.date_folder}"
        os.makedirs(self.file_base_path, exist_ok=True)
        self.scaler_filename = f"{self.file_base_path}/scaler.save"
        self.model_filename = f"{self.file_base_path}/model.h5"
        self.prediction_csv = f"{self.file_base_path}/predictions.csv"

    def get_building_name(self, building_id):
        """Fetch building name by ID."""
        building = Building.query.filter_by(id=building_id).first()
        return building.name if building else "unknown"

    def fetch_data(self):
        """Fetch data from the database."""
        if self.consumption_type == "electric":
            query = Electric.query
        elif self.consumption_type == "water":
            query = Water.query
        elif self.consumption_type == "naturalgas":
            query = NaturalGas.query
        else:
            raise ValueError("Invalid consumption type. Supported types: electric, water, naturalgas")

        if self.building_id:
            query = query.filter_by(building_id=self.building_id)

        data = query.order_by(Electric.date).all() if self.consumption_type == "electric" else query.order_by(Water.date).all()
        if not data:
            raise ValueError(f"No data found for {self.consumption_type} with building ID {self.building_id or 'all'}")

        return pd.DataFrame([(row.date, row.usage) for row in data], columns=['Date', 'Usage'])

    def prepare_data(self, df):
        """Prepare data for training."""
        data = df['Usage'].values.reshape(-1, 1)
        self.scaler = MinMaxScaler(feature_range=(0, 1))
        scaled_data = self.scaler.fit_transform(data)
        joblib.dump(self.scaler, self.scaler_filename)
        return scaled_data

    def train_model(self, data, epochs=50, batch_size=16):
        """Train a time series model using all data."""
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
        """Predict and save the next months' predictions to a CSV."""
        if not os.path.exists(self.model_filename):
            raise FileNotFoundError(f"Model file for {self.consumption_type} not found")
        if not os.path.exists(self.scaler_filename):
            raise FileNotFoundError(f"Scaler file for {self.consumption_type} not found")

        df = self.fetch_data()
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

    def get_predictions(self, months):
        """Return predictions for a specific number of months."""
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

    if not consumption_type:
        return jsonify({"error": "Consumption type (e.g., electric, water, naturalgas) is required"}), 400

    try:
        model = ConsumptionModel(consumption_type, building_id)
        df = model.fetch_data()
        data = model.prepare_data(df)
        model.train_model(data, epochs, batch_size)
        predictions_df = model.predict_and_save_csv()
        return jsonify({
            "message": f"Model for {consumption_type} trained and saved. Predictions for next 12 months saved to CSV.",
            "prediction_file": model.prediction_csv
        })
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


if __name__ == "__main__":
    app.run(debug=True)
