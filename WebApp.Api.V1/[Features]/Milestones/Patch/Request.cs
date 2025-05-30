using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Milestones.Patch;

namespace WebApp.Api.V1.Milestones.Patch;

public sealed record Request
{
    public MilestoneId Id { get; init; }
    public Patchable? Patch { get; init; }

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

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.Patch).NotNull().WithErrorCode(ErrorCodes.Required);
        RuleFor(a => a.Patch)
            .Must(a =>
                a is not null
                && (
                    a.Has(b => b.Title)
                    || a.Has(b => b.Color)
                    || a.Has(b => b.Emoji)
                    || a.Has(b => b.Description)
                    || a.Has(b => b.StatusId)
                    || a.Has(b => b.EndTime)
                    || a.Has(b => b.EndTimeZone)
                )
            )
            .WithErrorCode(ErrorCodes.InvalidValue);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial PatchMilestone ToCommand(this Request request);
}
