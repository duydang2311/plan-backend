using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.ChecklistItems.GetMany;

namespace WebApp.Api.V1.ChecklistItems.GetMany;

public sealed record Request : KeysetPagination<ChecklistItemId?>
{
    public IssueId? ParentIssueId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetChecklistItems ToCommand(this Request request);
}
