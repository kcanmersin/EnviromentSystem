import pandas as pd

# Excel dosyasının yolu
excel_path = "en.xlsx"
output_path = "en_corrected.xlsx"

# Tarih düzeltme fonksiyonu
def fix_dates(df):
    # 30.11.2022 tarihlerini düzelt
    count_30_11_2022 = 0
    for idx in df.index:
        if df.at[idx, "Date"] == "30.11.2022":
            count_30_11_2022 += 1
            if count_30_11_2022 == 2:  # İkinci kez karşılaştığımızda değiştir
                df.at[idx, "Date"] = "30.11.2023"

    # 25.02.2023 tarihlerini düzelt
    count_25_02_2023 = 0
    for idx in df.index:
        if df.at[idx, "Date"] == "25.02.2023":
            count_25_02_2023 += 1
            if count_25_02_2023 == 2:  # İkinci kez karşılaştığımızda değiştir
                df.at[idx, "Date"] = "25.02.2024"

    return df

# Excel dosyasını oku
excel_file = pd.ExcelFile(excel_path)
gas_sheet_names = [
    'ASEM LAB', 'REKTÖRLÜK KONUTT', 'YABANCI DİLLER KANTİN', 'KELEBEK KAFE',
    'ELEKTRONİK MÜHENDİSLİĞİ ', 'ENSTİTÜ BİNALARI-G3-H3-F3', 'MOLEKÜLER BİYOLOJİ VE GENETİK',
    'MALZEME BİLİM VE MÜHENDİSLİĞİ', 'İŞLETME BİNASI', 'KAPALI YÜZME HAVUZU',
    'KİMYA - ÇEVRE BÖLÜMÜ', 'KONGRE VE KÜLTÜR MERKEZİ', 'KÜTÜPHANE BİNASI',
    'MERKEZİ ARŞTIRMA LAB.', 'MİSAFİRHANE', 'PERSONEL YEMEKHANESİ ',
    'REKTÖRLÜK-ERASMUS BİNASI', 'TEKNOLOJİ TRANSFER OFİSİ', 'ANAOKULU KREŞ',
    'SÜMER BİNASI', 'SERA', 'KİMYA MÜH.', 'HARİTA MÜHENDİSLİĞİ-BİYOTEKNOLO',
    'YAPI İŞLERİ TEKNİK DAİRE BAŞKAN', 'ISI MERKEZİ', 'BİLGİSAYAR MÜHENDİSLİĞİ',
    'KAPALI SPOR SALONU ', 'ÖĞRENCİ İŞLERİ TEKNİK DAİRE BAŞ', 'HANGAR BİNASI',
    'KAGEM NANOTEKNOLOJİ FİZİK  '
]

# Yeni bir Excel writer oluştur
with pd.ExcelWriter(output_path, engine='openpyxl') as writer:
    for sheet_name in gas_sheet_names:
        # Sheet'i oku ve hataları yakala
        try:
            df = pd.read_excel(excel_file, sheet_name=sheet_name)
        except ValueError:
            print(f"Sheet '{sheet_name}' not found. Skipping.")
            continue

        # Tarih düzeltmeleri yap
        if "Date" in df.columns:
            df = fix_dates(df)

        # Dönüştürülmüş sheet'i yeni Excel dosyasına yaz
        df.to_excel(writer, sheet_name=sheet_name, index=False)

print(f"Düzeltmeler tamamlandı. Yeni dosya '{output_path}' olarak kaydedildi.")
