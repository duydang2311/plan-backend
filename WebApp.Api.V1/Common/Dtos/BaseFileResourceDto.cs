namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseFileResourceDto : BaseResourceDto
{
    public string? Key { get; init; }
}
