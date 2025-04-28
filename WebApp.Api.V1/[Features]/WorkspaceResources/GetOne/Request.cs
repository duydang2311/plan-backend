using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Api.V1.Common.Dtos;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.GetOne;

namespace WebApp.Api.V1.WorkspaceResources.GetOne;

public sealed record Request
{
    public ResourceId Id { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[UseStaticMapper(typeof(DtoMapper))]
public static partial class RequestMapper
{
    public static partial GetWorkspaceResource ToCommand(this Request request);
}
