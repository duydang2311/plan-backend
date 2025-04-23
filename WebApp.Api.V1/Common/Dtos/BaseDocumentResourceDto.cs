namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseDocumentResourceDto : BaseResourceDto
{
    public string? Content { get; init; }
}
