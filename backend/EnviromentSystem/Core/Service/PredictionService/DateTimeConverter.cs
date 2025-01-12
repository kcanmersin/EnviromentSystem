using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string[] _formats = new[]
    {
        "yyyy-MM-dd HH:mm:ss+0000", // Exact match for Flask's format
        "yyyy-MM-dd HH:mm:ss+00:00", // With colon
        "yyyy-MM-ddTHH:mm:sszzz",    // ISO 8601 with colon
        "yyyy-MM-ddTHH:mm:ssZ"        // ISO 8601 UTC
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();

        // Attempt to parse using predefined formats
        foreach (var format in _formats)
        {
            if (DateTime.TryParseExact(dateString, format, null, DateTimeStyles.None, out var date))
            {
                return date;
            }
        }

        // If parsing fails, attempt to insert a colon in the timezone offset
        if (dateString.Length > 19 && (dateString[19] == '+' || dateString[19] == '-'))
        {
            // Insert a colon before the last two digits of the timezone offset
            var modifiedDateString = dateString.Insert(22, ":"); // e.g., "2024-09-30 00:00:00+00:00"

            if (DateTime.TryParseExact(modifiedDateString, "yyyy-MM-dd HH:mm:sszzz", null, DateTimeStyles.None, out var modifiedDate))
            {
                return modifiedDate;
            }
        }

        throw new JsonException($"Invalid date format: {dateString}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // Match Flask's date format
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss+0000"));
    }
}