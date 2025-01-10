from flask import Flask, request, jsonify
from model import ConsumptionModel
import logging
import os
app = Flask(__name__)

# Logger'ı yapılandırma
logging.basicConfig(
    filename='app.log',
    level=logging.INFO,
    format='%(asctime)s:%(levelname)s:%(message)s'
)

@app.route('/train_model', methods=['POST'])
def train_model_endpoint():
    consumption_type = request.json.get('consumption_type', '').lower()
    building_id = request.json.get('building_id')
    threshold = float(request.json.get('threshold', 0.05))

    if not consumption_type:
        return jsonify({"error": "Consumption type (e.g., electric, water, naturalgas) is required"}), 400

    try:
        model = ConsumptionModel(consumption_type, building_id)
        df = model.fetch_data()
        
        # 1) Veri sayısı kontrolü (örnek eşik: 10)
        if df.shape[0] < 10:
            return jsonify({
                "error": f"Not enough data to train model. Minimum required is 10 rows, but got {df.shape[0]}."
            }), 400
        
        if df.empty:
            return jsonify({"error": "No data available for the requested consumption type and building ID"}), 400
        
        logging.info("Veri hazırlanıyor")
        scaled_features, scaled_target, feature_cols = model.prepare_data(df)
        
        logging.info("Model eğitiliyor")
        model.train_model(scaled_features, scaled_target, feature_cols, threshold=threshold)
        
        logging.info("Tahminler ve anomaliler kaydediliyor")
        return jsonify({
            "message": f"Model for {consumption_type} trained and saved.",
            "anomalies_detected": os.path.exists(model.anomaly_csv)
        })
    except Exception as e:
        logging.error(f"Hata oluştu: {str(e)}")
        return jsonify({"error": str(e)}), 500

@app.route('/get_prediction', methods=['POST'])
def get_prediction():
    consumption_type = request.json.get('consumption_type', '').lower()
    building_id = request.json.get('building_id')
    months = int(request.json.get('months', 12))

    if not consumption_type:
        return jsonify({"error": "Consumption type (e.g., electric, water, naturalgas) is required"}), 400

    try:
        model = ConsumptionModel(consumption_type, building_id)
        predictions = model.get_predictions(months)
        return jsonify(predictions.to_dict(orient='records'))
    except Exception as e:
        logging.error(f"Hata oluştu: {str(e)}")
        return jsonify({"error": str(e)}), 500

@app.route('/get_anomaly', methods=['POST'])
def get_anomaly():
    consumption_type = request.json.get('consumption_type', '').lower()
    building_id = request.json.get('building_id')
    threshold = request.json.get('threshold', 0.05)

    if not consumption_type:
        return jsonify({"error": "Consumption type (e.g., electric, water, naturalgas) is required"}), 400
    
    try:
        threshold = float(threshold)
        if not (0 < threshold < 1):
            return jsonify({"error": "Threshold must be a float between 0 and 1."}), 400

        model = ConsumptionModel(consumption_type, building_id)
        
        # 2) Anomali tespiti öncesi veri sayısı kontrolü
        df = model.fetch_data()
        if df.shape[0] < 10:
            return jsonify({
                "error": f"Not enough data to detect anomalies. Minimum required is 10 rows, but got {df.shape[0]}."
            }), 400

        anomaly_df = model.detect_anomalies(threshold)
        
        if anomaly_df.empty:
            return jsonify({"message": "No anomalies detected."})
        
        return jsonify(anomaly_df.to_dict(orient='records'))
    except ValueError as ve:
        logging.error(f"ValueError: {str(ve)}")
        return jsonify({"error": str(ve)}), 400
    except Exception as e:
        logging.error(f"Hata oluştu: {str(e)}")
        return jsonify({"error": str(e)}), 500

if __name__ == "__main__":
    app.run(debug=True)
