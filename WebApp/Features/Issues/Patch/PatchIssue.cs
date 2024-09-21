using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.Patch;

public sealed record PatchIssue : ICommand<OneOf<ValidationFailures, Success>>
{
    public required IssueId IssueId { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string Description { get; init; } = string.Empty;
        public IssuePriority Priority { get; init; }
        public StatusId StatusId { get; init; }
    }
}
