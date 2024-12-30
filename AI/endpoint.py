from flask import Flask, request, jsonify
from model import ConsumptionModel


app = Flask(__name__)

@app.route('/train_model', methods=['POST'])
def train_model_endpoint():
    consumption_type = request.json.get('consumption_type', '').lower()
    building_id = request.json.get('building_id')
    threshold = float(request.json.get('threshold', 0.05))

    if not consumption_type:
        return jsonify({"error": "Consumption type (e.g., electric, water, naturalgas) is required"}), 400

    try:
        # Modeli başlat
        model = ConsumptionModel(consumption_type, building_id)
        df = model.fetch_data()
        if df.empty:
            return jsonify({"error": "No data available for the requested consumption type and building ID"}), 400

        # Veri hazırlama
        print("Veri hazırlanıyor")
        scaled_features, scaled_target, feature_cols = model.prepare_data(df)

        # Modeli eğit
        print("Model eğitiliyor")
        model.train_model(scaled_features, scaled_target, feature_cols)  # Doğru çağrı

        # Tahminleri kaydet
        print("Tahminler kaydediliyor")
        model.predict_and_save_csv()

        # Anomali tespiti
        print("Anomaliler tespit ediliyor")
        model.detect_anomalies(threshold)

        # Sadece mesaj döndür
        return jsonify({"message": f"Model for {consumption_type} trained and saved."})
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
