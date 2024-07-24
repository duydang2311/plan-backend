using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueComments.Create;

public sealed record CreateIssueComment : ICommand<OneOf<ValidationFailures, IssueComment>>
{
    public required IssueId IssueId { get; init; }
    public required UserId AuthorId { get; init; }
    public required string Content { get; init; }
}
