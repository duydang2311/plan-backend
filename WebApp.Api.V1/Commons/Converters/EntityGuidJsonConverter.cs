using System.Text.Json;
using System.Text.Json.Serialization;
using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Commons.Converters;

public sealed class EntityGuidJsonConverter<T> : JsonConverter<T>
    where T : struct, IEntityGuid
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new T { Value = GuidToBase64JsonConverter.Instance.Read(ref reader, typeToConvert, options) };
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
        GuidToBase64JsonConverter.Instance.Write(writer, value.Value, options);

    public static ParseResult ValueParser(object? x)
    {
        var result = GuidToBase64JsonConverter.ValueParser(x);
        return result.IsSuccess
            ? new ParseResult(true, new T { Value = (Guid)result.Value! })
            : new ParseResult(false, default(T));
    }
}
