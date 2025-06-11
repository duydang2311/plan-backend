using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.GetMany;

namespace WebApp.Api.V1.Issues.GetMany;

public sealed record Request : Collective
{
    public TeamId? TeamId { get; init; }
    public ProjectId? ProjectId { get; init; }
    public string? Select { get; init; }
    public StatusId? StatusId { get; init; }
    public bool? NullStatusId { get; init; }
    public IReadOnlyCollection<IssueId>? ExcludeIssueIds { get; init; }
    public IssueId? ExcludeChecklistItemParentIssueId { get; init; }
    public string? StatusRankCursor { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetIssues ToCommand(this Request request);
}
