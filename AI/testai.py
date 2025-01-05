import subprocess
import json

# Flask API endpoint
url = "http://127.0.0.1:5000/train_model"

# Dosyalardan ID listelerini oku
with open('gas.txt', 'r') as gas_file:
    building_id_gas = [line.strip() for line in gas_file.readlines()]

with open('el.txt', 'r') as el_file:
    building_id_elec = [line.strip() for line in el_file.readlines()]

# Sabit threshold
threshold = 0.1

# Elektrik için modelleri eğit
for building_id in building_id_elec:
    payload = {
        "consumption_type": "electric",
        "building_id": building_id,
        "threshold": threshold
    }

    try:
        response = subprocess.run(
            [
                "curl",
                "-X", "POST",
                "-H", "Content-Type: application/json",
                "-d", json.dumps(payload),
                url
            ],
            capture_output=True,
            text=True
        )
        print(f"Response for electric building {building_id}: {response.stdout}")
    except Exception as e:
        print(f"Error for electric building {building_id}: {e}")

# Doğal gaz için modelleri eğit
for building_id in building_id_gas:
    payload = {
        "consumption_type": "naturalgas",
        "building_id": building_id,
        "threshold": threshold
    }

    try:
        response = subprocess.run(
            [
                "curl",
                "-X", "POST",
                "-H", "Content-Type: application/json",
                "-d", json.dumps(payload),
                url
            ],
            capture_output=True,
            text=True
        )
        print(f"Response for gas building {building_id}: {response.stdout}")
    except Exception as e:
        print(f"Error for gas building {building_id}: {e}")
