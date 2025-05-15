using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.ChecklistItems.Delete;

namespace WebApp.Api.V1.ChecklistItems.Delete;

public sealed record Request
{
    public ChecklistItemId Id { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial DeleteChecklistItem ToCommand(this Request request);
}
