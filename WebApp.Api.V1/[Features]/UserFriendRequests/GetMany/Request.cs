using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.UserFriendRequests.GetMany;

namespace WebApp.Api.V1.UserFriendRequests.GetMany;

public sealed record Request : Collective
{
    public UserId? SenderId { get; init; }
    public UserId? ReceiverId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetUserFriendRequests ToCommand(this Request request);
}
