using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Projects.Create;

public sealed record Response
{
    public ProjectId Id { get; init; }
    public required string Identifier { get; init; }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this Project project);
}
