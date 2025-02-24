using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueComments.Create;

public sealed record CreateIssueComment : ICommand<OneOf<ValidationFailures, Success>>
{
    public required IssueId IssueId { get; init; }
    public required UserId AuthorId { get; init; }
    public required string Content { get; init; }
}
