using FastEndpoints;
using NodaTime;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Milestones.Patch;

public sealed record PatchMilestone : ICommand<OneOf<NotFoundError, InvalidPatchError, Success>>
{
    public required MilestoneId Id { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string? Title { get; init; }
        public string? Color { get; init; }
        public string? Emoji { get; init; }
        public string? Description { get; init; }
        public MilestoneStatusId? StatusId { get; init; }
        public Instant? EndTime { get; init; }
        public string? EndTimeZone { get; init; }
    }
}
