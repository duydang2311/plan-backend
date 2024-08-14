using System.Text.Json;
using System.Text.Json.Serialization;
using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Converters;

public sealed class EntityIdJsonConverter<TId, TValue> : JsonConverter<TId>
    where TId : struct, IEntityId<TValue>
{
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = ((JsonConverter<TValue>)JsonSerializerOptions.Default.GetConverter(typeof(TValue))).Read(
            ref reader,
            typeToConvert,
            options
        );
        return new TId { Value = value! };
    }

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options) =>
        ((JsonConverter<TValue>)JsonSerializerOptions.Default.GetConverter(typeof(TValue))).Write(
            writer,
            value.Value,
            options
        );

    public static ParseResult ValueParser(object? x)
    {
        if (x is null)
        {
            return new ParseResult(false, default);
        }

        if (x.GetType() == typeof(TValue))
        {
            return new ParseResult(true, new TId { Value = (TValue)x });
        }

        throw new NotImplementedException();
    }
}
