using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.Patch;

namespace WebApp.Api.V1.Issues.Patch;

public sealed record Request
{
    public IssueId IssueId { get; init; }
    public Patchable? Patch { get; init; } = null!;

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

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Patch).NotNull().WithErrorCode(ErrorCodes.Required);
        When(
            a => a.Patch is not null,
            () =>
            {
                RuleFor(a => a.Patch)
                    .Must(
                        (a) =>
                            a!.Has(b => b.Title)
                            || a.Has(b => b.Description)
                            || a.Has(b => b.Priority)
                            || a.Has(b => b.StatusId)
                            || a.Has(b => b.StatusRank)
                            || a.Has(b => b.StartTime)
                            || a.Has(b => b.EndTime)
                            || a.Has(b => b.TimelineZone)
                            || a.Has(b => b.MilestoneId)
                    )
                    .WithErrorCode(ErrorCodes.InvalidValue);
            }
        );
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial PatchIssue ToCommand(this Request request);
}
