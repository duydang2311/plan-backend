using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueAudits.GetMany;

public sealed record GetIssueAudits : Collective, IKeysetPagination<long?>, ICommand<PaginatedList<IssueAudit>>
{
    public required IssueId IssueId { get; init; }
    public string? Select { get; init; }
    public long? Cursor { get; init; }
}
