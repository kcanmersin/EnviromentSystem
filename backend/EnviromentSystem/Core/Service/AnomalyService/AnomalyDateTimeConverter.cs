using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

public class AnomalyDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string[] _formats = new[]
    {
        "ddd, dd MMM yyyy HH:mm:ss GMT",
        "yyyy-MM-dd HH:mm:ss+00:00", 
        "yyyy-MM-ddTHH:mm:sszzz"  
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        if (DateTime.TryParseExact(dateString, _formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date))
        {
            return date;
        }
        throw new JsonException($"Invalid date format: {dateString}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("ddd, dd MMM yyyy HH:mm:ss GMT"));
    }
}
