using System.Text.Json;
using System.Text.Json.Serialization;
using FastEndpoints;
using WebApp.SharedKernel.Models;

namespace WebApp.Api.V1.Commons.Converters;

public class OrderableArrayJsonConverter : JsonConverter<Orderable[]>
{
    public override Orderable[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader
                .GetString()
                ?.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x) && x.Length > 1)
                .Select(x =>
                {
                    if (x![0] == '-')
                    {
                        return new Orderable { Name = x[1..].Trim(), Order = SharedKernel.Constants.Order.Descending };
                    }
                    return new Orderable { Name = x.Trim(), Order = SharedKernel.Constants.Order.Ascending };
                })
                .ToArray() ?? [];
    }

    public override void Write(Utf8JsonWriter writer, Orderable[] value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(
            string.Join(
                ',',
                value.Select(x => x.Order == SharedKernel.Constants.Order.Ascending ? x.Name : '-' + x.Name)
            )
        );
    }

    public static ParseResult ValueParser(object? x)
    {
        return new ParseResult(
            true,
            x?.ToString()
                ?.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x) && x.Length > 1)
                .Select(x =>
                {
                    if (x![0] == '-')
                    {
                        return new Orderable { Name = x[1..].Trim(), Order = SharedKernel.Constants.Order.Descending };
                    }
                    return new Orderable { Name = x.Trim(), Order = SharedKernel.Constants.Order.Ascending };
                })
                .ToArray() ?? []
        );
    }
}
