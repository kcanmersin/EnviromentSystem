import os
import pandas as pd

def check_negative_predictions(base_dir):
    negative_files = []

    # Walk through the directory
    for root, _, files in os.walk(base_dir):
        for file in files:
            # if file == 'predictions.csv':
            #if file ends with 'predictions.csv'

            if file.endswith('predictions.csv'):
                file_path = os.path.join(root, file)

                # Read the CSV and check for negative values
                try:
                    df = pd.read_csv(file_path)
                    if (df['Predicted_Usage'] < 0).any():
                        negative_files.append(file_path)
                        #also print which 
                except Exception as e:
                    print(f"Error reading {file_path}: {e}")

    return negative_files

if __name__ == "__main__":
    base_dirs = [
        r"D:\\code\\EnviromentSystem\\AI\\Consumptions\\Electric",
        r"D:\\code\\EnviromentSystem\\AI\\Consumptions\\Naturalgas"
        # "aznegatiflicons\Consumptions\Electric",
        # "aznegatiflicons\Consumptions\Naturalgas"
        # r"D:\code\EnviromentSystem\AI\aznegatiflicons\Consumptions\Electric",
        # r"D:\code\EnviromentSystem\AI\aznegatiflicons\Consumptions\Naturalgas"
    ]

    for base_dir in base_dirs:
        print(f"Checking directory: {base_dir}")
        negative_files = check_negative_predictions(base_dir)

        if negative_files:
            print(f"Files with negative predicted usage in {base_dir}:\n")
            for file in negative_files:
                print(f"  - {file}")
        else:
            print(f"No files with negative predicted usage found in {base_dir}.\n")
