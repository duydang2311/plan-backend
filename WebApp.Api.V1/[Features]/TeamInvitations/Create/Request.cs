using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.TeamInvitations.Create;

namespace WebApp.Api.V1.TeamInvitations.Create;

public sealed record Request
{
    public TeamId TeamId { get; init; }
    public UserId? MemberId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.MemberId).NotNull();
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial CreateTeamInvitation ToCommand(this Request request);
}
