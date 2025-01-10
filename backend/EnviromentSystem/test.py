import pandas as pd
from sqlalchemy import create_engine, text


# Elektrik verilerini al
with engine.connect() as connection:
    electric_query = """
    SELECT b."Id" AS "BuildingId", b."Name" AS "BuildingName", EXTRACT(YEAR FROM e."Date") AS "Year", EXTRACT(MONTH FROM e."Date") AS "Month"
    FROM "Electrics" e
    JOIN "Buildings" b ON e."BuildingId"::text = b."Id"::text
    WHERE e."IsDeleted" = FALSE
    GROUP BY b."Id", b."Name", EXTRACT(YEAR FROM e."Date"), EXTRACT(MONTH FROM e."Date")
    ORDER BY b."Name", "Year", "Month"
    """
    electric_data = pd.read_sql(text(electric_query), connection)

    gas_query = """
    SELECT b."Id" AS "BuildingId", b."Name" AS "BuildingName", EXTRACT(YEAR FROM g."Date") AS "Year", EXTRACT(MONTH FROM g."Date") AS "Month"
    FROM "NaturalGasUsages" g
    JOIN "Buildings" b ON g."BuildingId"::text = b."Id"::text
    WHERE g."IsDeleted" = FALSE
    GROUP BY b."Id", b."Name", EXTRACT(YEAR FROM g."Date"), EXTRACT(MONTH FROM g."Date")
    ORDER BY b."Name", "Year", "Month"
    """
    gas_data = pd.read_sql(text(gas_query), connection)

# Eksik ayları bulmak için işlem
def find_missing_months(building_data):
    results = []
    for year in sorted(building_data["Year"].unique()):
        year_data = building_data[building_data["Year"] == year]
        months_present = sorted(year_data["Month"].unique())
        
        # Sıralı ayları kontrol et
        for i in range(len(months_present) - 1):
            if months_present[i + 1] != months_present[i] + 1:
                missing_month = months_present[i] + 1
                while missing_month < months_present[i + 1]:
                    results.append((int(year), int(missing_month)))
                    missing_month += 1
    return results

# Elektrik için eksik ayları kontrol et ve bastır
print("Eksik Aylar (Elektrik):")
for building in electric_data["BuildingName"].unique():
    building_data = electric_data[electric_data["BuildingName"] == building]
    building_id = building_data["BuildingId"].iloc[0]  # Bina ID'si alınır
    missing_months = find_missing_months(building_data)
    if missing_months:
        print(f"\nBina: {building} (ID: {building_id})")
        for year, month in missing_months:
            print(f"  Eksik Yıl: {year}, Ay: {month}")

# Doğalgaz için eksik ayları kontrol et ve bastır
print("\nEksik Aylar (Doğalgaz):")
for building in gas_data["BuildingName"].unique():
    building_data = gas_data[gas_data["BuildingName"] == building]
    building_id = building_data["BuildingId"].iloc[0]  # Bina ID'si alınır
    missing_months = find_missing_months(building_data)
    if missing_months:
        print(f"\nBina: {building} (ID: {building_id})")
        for year, month in missing_months:
            print(f"  Eksik Yıl: {year}, Ay: {month}")
