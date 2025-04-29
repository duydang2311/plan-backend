namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseResourceFileDto
{
    public string? Key { get; init; }
    public string? OriginalName { get; init; }
    public long? Size { get; init; }
    public string? MimeType { get; init; }
}
