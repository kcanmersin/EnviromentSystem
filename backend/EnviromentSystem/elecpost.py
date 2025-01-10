import pandas as pd
from sqlalchemy import create_engine, exc
import uuid
from datetime import datetime, timezone
import re  # Sütunları düzgün ayrıştırmak için

# Fonksiyon: Numara dönüştürme
def normalize_number(value):
    try:
        value = value.replace(".", "").replace(",", ".")
        return float(value)
    except ValueError:
        raise ValueError(f"Invalid numeric value: {value}")

# Fonksiyon: Txt dosyasından gaz verilerini okuma
def read_gas_data(file_path, building_id):
    try:
        with open(file_path, 'r', encoding='utf-8') as file:
            lines = file.readlines()
        
        data_list = []
        for line in lines:
            if line.strip():  # Boş satırları atla
                # Sütunları en az iki boşluğa göre böl
                parts = re.split(r'\s{2,}', line.strip())
                try:
                    if len(parts) < 6:  # Beklenen sütun sayısını kontrol et
                        raise ValueError(f"Incorrect number of columns: {len(parts)} (Expected: 6+)")

                    month = parts[0]
                    date = parts[1]
                    initial_meter_value = normalize_number(parts[3])
                    final_meter_value = normalize_number(parts[4])
                    usage = normalize_number(parts[5])
                    sm3_value = normalize_number(parts[6])
                    
                    data_list.append({
                        "Id": str(uuid.uuid4()),
                        "BuildingId": building_id,
                        "Date": datetime.strptime(date, "%d.%m.%Y").replace(tzinfo=timezone.utc),
                        "InitialMeterValue": initial_meter_value,
                        "FinalMeterValue": final_meter_value,
                        "Usage": usage,
                        "SM3Value": sm3_value,
                        "CreatedDate": datetime.now(timezone.utc),
                        "IsDeleted": False
                    })
                except (IndexError, ValueError) as e:
                    print(f"Error parsing line, skipping: {line.strip()}\nError: {e}")
        return data_list
    except FileNotFoundError as e:
        print(f"File not found: {file_path}\nError: {e}")
        return []
    except Exception as e:
        print(f"Unexpected error while reading the file: {file_path}\nError: {e}")
        return []

# Fonksiyon: Veritabanına veri ekleme
def insert_gas_data_to_db(data_list):
    if not data_list:
        print("No data to insert into the database.")
        return
    
    try:
        df = pd.DataFrame(data_list)
        df.to_sql('NaturalGasUsages', engine, if_exists='append', index=False)
        print("Gas data successfully inserted into the database.")
    except exc.SQLAlchemyError as e:
        print(f"Error during database insertion: {e}")

# Ana fonksiyon
def main():
    # Dosya yolu ve bina ID
    file_path = "sümergaz.txt"  # Txt dosyasının yolu
    building_id = "806c0ac2-a7fd-4b11-ae86-6513268d4325"  # Hedef bina ID

    print("Reading gas data from file...")
    gas_data = read_gas_data(file_path, building_id)
    if gas_data:
        print(f"Read {len(gas_data)} records. Inserting into database...")
        insert_gas_data_to_db(gas_data)
    else:
        print("No data to process. Please check the file or its format.")

if __name__ == "__main__":
    main()
