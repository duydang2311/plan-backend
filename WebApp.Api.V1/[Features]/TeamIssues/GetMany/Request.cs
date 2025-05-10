using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.TeamIssues.GetMany;

namespace WebApp.Api.V1.TeamIssues.GetMany;

public sealed record Request : Collective
{
    public TeamId? TeamId { get; init; }
    public IssueId? IssueId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.TeamId)
            .NotNull()
            .When(x => !x.IssueId.HasValue)
            .WithErrorCode("required")
            .WithMessage("Either TeamId or IssueId must be provided.");

        RuleFor(x => x.IssueId)
            .NotNull()
            .When(x => !x.TeamId.HasValue)
            .WithErrorCode("required")
            .WithMessage("Either TeamId or IssueId must be provided.");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetTeamIssues ToCommand(this Request request);
}
