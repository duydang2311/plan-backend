using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.ChatMessages.Create;

namespace WebApp.Api.V1.ChatMessages.Create;

public sealed record Request
{
    public ChatId? ChatId { get; init; }
    public string? Content { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(r => r.ChatId).NotNull().WithErrorCode("required");
        RuleFor(r => r.Content).NotEmpty().WithErrorCode("required");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    [MapProperty(nameof(Request.RequestingUserId), nameof(CreateChatMessage.SenderId))]
    public static partial CreateChatMessage ToCommand(this Request request);
}
