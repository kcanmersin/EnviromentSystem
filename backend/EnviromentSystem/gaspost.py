import pandas as pd
from sqlalchemy import create_engine, exc
import uuid
from datetime import datetime, timezone


def normalize_number(value):
    try:
        # Türkçe sayı formatından İngilizce formatına çevir
        value = value.replace(".", "").replace(",", ".").strip()
        return float(value)
    except ValueError:
        raise ValueError(f"Invalid numeric value: {value}")

def read_gas_data(file_path, building_id):
    try:
        with open(file_path, 'r', encoding='utf-8') as file:
            lines = file.readlines()

        data_list = []
        for line in lines:
            if line.strip():  # Boş satırları atla
                parts = line.split()
                try:
                    # Parçaları doğru şekilde ayırarak işle
                    month = parts[0]
                    date_str = parts[1]
                    initial_meter_value = normalize_number(parts[2])
                    final_meter_value = normalize_number(parts[3])
                    usage = normalize_number(parts[4])
                    sm3_value = normalize_number(parts[5])

                    data_list.append({
                        "Id": str(uuid.uuid4()),
                        "BuildingId": building_id,
                        "Date": datetime.strptime(date_str, "%d.%m.%Y").replace(tzinfo=timezone.utc),
                        "InitialMeterValue": initial_meter_value,
                        "FinalMeterValue": final_meter_value,
                        "Usage": usage,
                        "SM3Value": sm3_value,
                        "CreatedDate": datetime.now(timezone.utc),
                        "IsDeleted": False
                    })
                except (IndexError, ValueError) as e:
                    print(f"Error processing line: {line.strip()}\nError: {e}")
                    # Hatalı bir satır bulunduğu anda tüm işlemi iptal et
                    return []
        return data_list
    except FileNotFoundError as e:
        print(f"File not found: {file_path}\nError: {e}")
        return []
    except Exception as e:
        print(f"Unexpected error while reading the file: {file_path}\nError: {e}")
        return []

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

def main():
        # file_path = "sümergaz.txt"
    # building_id = "806c0ac2-a7fd-4b11-ae86-6513268d4325"
    # Dosya yolu ve bina ID'si
    # file_path = "bilgaz.txt"
    # building_id = "16d947ae-6ee6-48a7-bb6c-af043d8b805e"

    # file_path = "rekkonut.txt"
    # building_id = "e823517a-eff6-4381-99fd-36f0710dbbf3"

    file_path = "asem.txt"
    building_id = "42b96819-1c8a-41c3-81d4-4619b75fc2eb"
        # file_path = "bilgaz.txt"
    # building_id = "16d947ae-6ee6-48a7-bb6c-af043d8b805e"

    print("Reading gas data from file...")
    gas_data = read_gas_data(file_path, building_id)
    if gas_data:
        print(f"Read {len(gas_data)} records. Inserting into database...")
        insert_gas_data_to_db(gas_data)
    else:
        print("No data to process. Please check the file or its format.")

if __name__ == "__main__":
    main()
