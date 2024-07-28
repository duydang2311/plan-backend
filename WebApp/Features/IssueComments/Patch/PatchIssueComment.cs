using FastEndpoints;
using Json.Patch;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueComments.Patch;

public sealed record PatchIssueComment : ICommand<OneOf<ValidationFailures, IssueComment>>
{
    public required IssueCommentId IssueCommentId { get; init; }
    public required JsonPatch Patch { get; init; }
}
