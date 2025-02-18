using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;

namespace WebApp.Features.IssueAudits.Delete;

public sealed record DeleteIssueAudit : ICommand<OneOf<NotFoundError, Success>>
{
    public long Id { get; init; }
}
