import matplotlib.pyplot as plt
import pandas as pd
import io

# String veriyi Python koduna al
data = """
Year  Month      Usage
2022      1  175345.00
2022      2  159816.00
2022      3  159773.00
2022      4  103993.00
2022      5   57872.00
2022      6   14762.00
2022      7    8563.00
2022      8   10751.00
2022      9   11305.00
2022     10   36598.00
2022     11  183222.00
2022      2  159816.00
2022      3  159773.00
2022      4  103993.00
2022      5   57872.00
2022      6   14762.00
2022      7    8563.00
2022      8   10751.00
2022      9   11305.00
2022     10   36598.00
2022     11  183222.00
2022     12  131765.00
2023      1  159658.00
2023      2  305768.00
2023      3  103257.00
2023      4   88152.00
2023      5   29294.00
2022     12  131765.00
2023      1  159658.00
2023      2  305768.00
2023      3  103257.00
2023      4   88152.00
2023      5   29294.00
2023      6   13429.00
2023      7    7622.00
2023      8    7364.00
2023      6   13429.00
2023      7    7622.00
2023      8    7364.00
2023      9    9857.00
2023     10   19861.00
2023     12  135011.00
2023     12  135011.00
2024      1  153742.00
2024      2  153742.00
2024      1  153742.00
2024      2  153742.00
2024      3  153808.00
2024      4   53397.00
2024      5   48978.00
2024      6   12765.00
2024      7   11228.00
2024      8   10624.00
2024      9   17817.00
"""

# String'i DataFrame'e dönüştür
df = pd.read_csv(io.StringIO(data), delim_whitespace=True)

# Tekrarlayan satırları kaldır
df = df.drop_duplicates()

# Tarih sütununu oluştur
df['Date'] = pd.to_datetime(df['Year'].astype(str) + '-' + df['Month'].astype(str) + '-01')

# Kullanım verisini tarihe göre sıralama
df = df.sort_values('Date')

# Grafik çizimi
plt.figure(figsize=(12, 6))
plt.plot(df['Date'], df['Usage'], marker='o', linestyle='-', color='b', label='Usage')
plt.title('Natural Gas Usage Over Time')
plt.xlabel('Date')
plt.ylabel('Usage')
plt.xticks(rotation=45)
plt.grid(True)
plt.legend()
plt.tight_layout()
plt.show()
