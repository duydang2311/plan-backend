using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApp.Common.Models;

namespace WebApp.Api.V1.Common.Converters;

// ref: https://stackoverflow.com/a/72910769
public sealed class PatchableJsonConverter : JsonConverter<Patchable>
{
    public override bool CanConvert(Type typeToConvert) => typeof(Patchable).IsAssignableFrom(typeToConvert);

    public override Patchable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var patchable = (Patchable)Activator.CreateInstance(typeToConvert)!;
        var properties = typeToConvert
            .GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty
            )
            .ToDictionary(p => options.PropertyNamingPolicy?.ConvertName(p.Name) ?? p.Name, StringComparer.Ordinal);

        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject:
                    return patchable;

                case JsonTokenType.PropertyName:
                    if (!properties.TryGetValue(reader.GetString()!, out var propertyInfo))
                    {
                        break;
                    }
                    reader.Read();
                    propertyInfo.SetValue(
                        patchable,
                        JsonSerializer.Deserialize(ref reader, propertyInfo.PropertyType, options)
                    );
                    patchable.PresentProperties.Add(propertyInfo.Name);
                    break;
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Patchable value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
