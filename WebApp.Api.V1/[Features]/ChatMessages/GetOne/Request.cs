using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.ChatMessages.GetOne;

namespace WebApp.Api.V1.ChatMessages.GetOne;

public sealed record Request : KeysetPagination<ChatMessageId?>
{
    public ChatMessageId ChatMessageId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetChatMessage ToCommand(this Request request);
}
