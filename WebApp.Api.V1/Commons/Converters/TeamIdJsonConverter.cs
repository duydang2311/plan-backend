using System.Text.Json;
using System.Text.Json.Serialization;
using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Commons.Converters;

public sealed class TeamIdJsonConverter : JsonConverter<TeamId>
{
    public override TeamId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new TeamId(GuidToBase64JsonConverter.Instance.Read(ref reader, typeToConvert, options));
    }

    public override void Write(Utf8JsonWriter writer, TeamId value, JsonSerializerOptions options) =>
        GuidToBase64JsonConverter.Instance.Write(writer, value.Value, options);

    public static ParseResult ValueParser(object? x)
    {
        var result = GuidToBase64JsonConverter.ValueParser(x);
        return result.IsSuccess
            ? new ParseResult(true, new TeamId((Guid)result.Value!))
            : new ParseResult(false, TeamId.Empty);
    }
}
