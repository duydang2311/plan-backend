using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueComments.Patch;

public sealed record PatchIssueComment : ICommand<OneOf<ValidationFailures, Success>>
{
    public required IssueCommentId IssueCommentId { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string Content { get; init; } = string.Empty;
    }
}
