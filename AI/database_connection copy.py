# database_connection.py
import psycopg2



def get_db_connection():
    return psycopg2.connect(**DB_PARAMS)
