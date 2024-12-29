using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Workspaces.GetMany;

namespace WebApp.Api.V1.Workspaces.GetMany;

public sealed record Request : Collective
{
    public UserId? UserId { get; init; }
    public string? Select { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreTarget(nameof(GetWorkspaces.WorkspaceId))]
    [MapperIgnoreTarget(nameof(GetWorkspaces.Path))]
    public static partial GetWorkspaces ToCommand(this Request request);
}
