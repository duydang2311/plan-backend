using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Api.V1.Common.Dtos;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.GetMany;

namespace WebApp.Api.V1.WorkspaceResources.GetMany;

public sealed record Request : KeysetPagination<ResourceId?>
{
    public WorkspaceId WorkspaceId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[UseStaticMapper(typeof(DtoMapper))]
public static partial class RequestMapper
{
    public static partial GetWorkspaceResources ToCommand(this Request request);
}
