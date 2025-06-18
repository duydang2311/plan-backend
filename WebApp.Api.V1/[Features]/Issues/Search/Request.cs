using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.Search;

namespace WebApp.Api.V1.Issues.Search;

public sealed record Request : Collective
{
    public ProjectId? ProjectId { get; init; }
    public string? Query { get; init; }
    public string? Select { get; init; }
    public IReadOnlyCollection<IssueId>? ExcludeIssueIds { get; init; }
    public IssueId? ExcludeChecklistItemParentIssueId { get; init; }
    public float? Threshold { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.ProjectId).NotNull().WithErrorCode(ErrorCodes.Required);
        RuleFor(x => x.Query).NotEmpty().WithErrorCode(ErrorCodes.Required);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial SearchIssues ToCommand(this Request request);
}
