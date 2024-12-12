using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueAssignees.Delete;

public sealed record DeleteIssueAssignee : ICommand<OneOf<NotFoundError, Success>>
{
    public required IssueId IssueId { get; init; }
    public required UserId UserId { get; init; }
}
