using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sqids;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Converters;

public sealed class EncodedEntityIdLongJsonConverter<TId>(SqidsEncoder<long> encoder) : JsonConverter<TId>
    where TId : struct, IEntityId<long>
{
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString() ?? throw new JsonException("Value cannot be null.");

        Span<char> span = stackalloc char[reader.ValueSpan.Length];
        Encoding.ASCII.GetChars(reader.ValueSpan, span);
        var ids = encoder.Decode(span);
        if (ids.Count != 1)
        {
            throw new JsonException("Invalid encoded ID");
        }
        return new TId { Value = ids[0] };
    }

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options) =>
        writer.WriteStringValue(encoder.Encode(value.Value));
}
