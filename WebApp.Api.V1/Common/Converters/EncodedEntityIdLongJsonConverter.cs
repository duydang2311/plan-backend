using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApp.Common.IdEncoding;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Converters;

public sealed class EncodedEntityIdLongJsonConverter<TId>(IIdEncoder idEncoder) : JsonConverter<TId>
    where TId : struct, IEntityId<long>
{
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString() ?? throw new JsonException("Value cannot be null.");

        Span<char> span = stackalloc char[reader.ValueSpan.Length];
        Encoding.ASCII.GetChars(reader.ValueSpan, span);
        if (!idEncoder.TryDecode(span, out var id))
        {
            throw new JsonException("Invalid encoded ID");
        }
        return new TId { Value = id };
    }

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options) =>
        writer.WriteStringValue(idEncoder.Encode(value.Value));
}
