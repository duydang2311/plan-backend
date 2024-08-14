using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.TeamMembers.UpdateRole;

namespace WebApp.Api.V1.TeamMembers.UpdateRole;

public sealed record Request
{
    public TeamId TeamId { get; init; }
    public UserId MemberId { get; init; }
    public TeamRoleId? TeamRoleId { get; init; }
    public string? RoleName { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.TeamRoleId).NotNull().When(a => string.IsNullOrEmpty(a.RoleName));
        RuleFor(a => a.RoleName).NotNull().When(a => a.TeamRoleId is null);
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial UpdateTeamMemberRole ToCommand(this Request request);
}
