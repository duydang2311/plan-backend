using System.Text.Json;
using System.Text.Json.Serialization;
using FastEndpoints;
using WebApp.SharedKernel.Models;

namespace WebApp.Api.V1.Commons.Converters;

public sealed class UserIdJsonConverter : JsonConverter<UserId>
{
    public override UserId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new UserId(GuidToBase64JsonConverter.Instance.Read(ref reader, typeToConvert, options));
    }

    public override void Write(Utf8JsonWriter writer, UserId value, JsonSerializerOptions options) =>
        GuidToBase64JsonConverter.Instance.Write(writer, value.Value, options);

    public static ParseResult ValueParser(object? x)
    {
        var result = GuidToBase64JsonConverter.ValueParser(x);
        return result.IsSuccess
            ? new ParseResult(true, new UserId((Guid)result.Value!))
            : new ParseResult(false, UserId.Empty);
    }
}
