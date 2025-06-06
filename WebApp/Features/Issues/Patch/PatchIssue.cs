using FastEndpoints;
using NodaTime;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.Patch;

public sealed record PatchIssue : ICommand<OneOf<ValidationFailures, NotFoundError, Success>>
{
    public required IssueId IssueId { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string? Title { get; init; }
        public string? Description { get; init; }
        public IssuePriority? Priority { get; init; }
        public StatusId? StatusId { get; init; }
        public string? StatusRank { get; init; }
        public Instant? StartTime { get; init; }
        public Instant? EndTime { get; init; }
        public string? TimelineZone { get; init; }
        public MilestoneId? MilestoneId { get; init; }
    }
}
