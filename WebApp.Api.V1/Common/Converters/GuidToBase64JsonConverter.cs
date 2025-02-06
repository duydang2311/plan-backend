using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FastEndpoints;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace WebApp.Api.V1.Common.Converters;

public sealed class GuidToBase64JsonConverter : JsonConverter<Guid>
{
    public static readonly GuidToBase64JsonConverter Instance = new();

    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }
        var span = reader.ValueSpan;
        if (Utf8Parser.TryParse(span, out Guid guid, out var bytesConsumed) && span.Length == bytesConsumed)
        {
            return guid;
        }

        var value = reader.GetString() ?? throw new JsonException();
        if (Guid.TryParseExact(value, "D", out guid))
        {
            return guid;
        }

        byte[] bytes;
        try
        {
            bytes = Base64UrlTextEncoder.Decode(value);
        }
        catch
        {
            throw new JsonException();
        }
        if (bytes.Length != 16)
        {
            throw new JsonException();
        }
        return new Guid(bytes);
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        if (value == Guid.Empty)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(Base64UrlTextEncoder.Encode(value.ToByteArray()));
        }
    }

    public static ParseResult ValueParser(StringValues x)
    {
        var value = x.FirstOrDefault();
        if (value is null)
        {
            return new ParseResult(false, Guid.Empty);
        }

        if (Guid.TryParseExact(value, "D", out var guid))
        {
            return new ParseResult(true, guid);
        }

        byte[] bytes;
        try
        {
            bytes = Base64UrlTextEncoder.Decode(value);
        }
        catch
        {
            return new ParseResult(false, Guid.Empty);
        }
        return bytes.Length == 16 ? new ParseResult(true, new Guid(bytes)) : new ParseResult(false, Guid.Empty);
    }
}
