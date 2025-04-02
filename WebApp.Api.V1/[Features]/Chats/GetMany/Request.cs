using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Chats.GetMany;

namespace WebApp.Api.V1.Chats.GetMany;

public sealed record Request : Collective
{
    public UserId? UserId { get; init; }
    public string? Select { get; init; }
    public string? SelectLastChatMessage { get; init; }
    public UserId? FilterChatMemberId { get; init; }
    public string? SelectChatMember { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetChats ToCommand(this Request request);
}
