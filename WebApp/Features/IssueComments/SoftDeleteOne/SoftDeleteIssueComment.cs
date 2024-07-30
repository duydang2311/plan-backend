using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueComments.SoftDeleteOne;

public sealed record SoftDeleteIssueComment : ICommand<OneOf<ValidationFailures, Success>>
{
    public required IssueCommentId IssueCommentId { get; init; }
}
