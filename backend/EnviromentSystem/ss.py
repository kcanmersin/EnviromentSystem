import os

def process_files(base_output_file_name, line_limit):
    file_counter = 1
    current_line_count = 0
    output_file_path = f"{base_output_file_name}_{file_counter}.txt"
    
    with open(output_file_path, 'w', encoding='utf-8') as output_file:
        for root, dirs, files in os.walk("."):
            if "Migrations" in root.split(os.sep):
                continue
            for file in files:
                if file.endswith(".cs"):
                    source_path = os.path.join(root, file)
                    try:
                        with open(source_path, 'r', encoding='utf-8') as source_file:
                            for line in source_file:
                                if "using" not in line:
                                    if current_line_count >= line_limit:
                                        output_file.close()
                                        file_counter += 1
                                        output_file_path = f"{base_output_file_name}_{file_counter}.txt"
                                        output_file = open(output_file_path, 'w', encoding='utf-8')
                                        current_line_count = 0
                                    output_file.write(line)
                                    current_line_count += 1
                    except UnicodeDecodeError:
                        print(f"Encoding error in file: {source_path}, skipped.")
                    except Exception as e:
                        print(f"Error processing file: {source_path}, error: {e}")
        output_file.close()

# Usage
base_output_file_name = "combined_output"
line_limit = 600
process_files(base_output_file_name, line_limit)
