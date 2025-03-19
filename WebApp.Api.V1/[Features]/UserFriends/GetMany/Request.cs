using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.UserFriends.GetMany;

namespace WebApp.Api.V1.UserFriends.GetMany;

public sealed record Request : Collective
{
    public UserId? UserId { get; init; }
    public UserId? FriendId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetUserFriends ToCommand(this Request request);
}
