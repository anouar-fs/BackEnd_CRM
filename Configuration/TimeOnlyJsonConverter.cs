namespace BackEnd.Configuration;
using Newtonsoft.Json;
using System;

public class TimeOnlyNewtonsoftConverter : JsonConverter<TimeOnly>
{
    private const string Format = "HH:mm";

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(Format));
    }

    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return TimeOnly.ParseExact((string)reader.Value!, Format);
    }
}