using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Chats.Create;

namespace WebApp.Api.V1.Chats.Create;

public sealed record Request
{
    public UserId[] MemberIds { get; init; } = [];

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    [MapProperty(nameof(Request.RequestingUserId), nameof(CreateChat.OwnerId))]
    public static partial CreateChat ToCommand(this Request request);
}
