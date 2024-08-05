using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueComments.GetOne;

public sealed record GetIssueComment : ICommand<OneOf<None, IssueComment>>
{
    public required IssueCommentId IssueCommentId { get; init; }
    public string? Select { get; init; }
}
