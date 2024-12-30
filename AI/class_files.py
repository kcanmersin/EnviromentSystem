import pandas as pd
from database_connection import get_db_connection


class Water:
    @staticmethod
    def fetch_data(building_id=None):
        conn = get_db_connection()
        cursor = conn.cursor()
        query = 'SELECT "Date", "Usage" FROM "Waters"'
        if building_id:
            query += ' WHERE "BuildingId" = %s'
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
            query += ' WHERE "BuildingId" = %s'
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
            query += ' WHERE "BuildingId" = %s'
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
