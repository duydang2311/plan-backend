using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.Milestones.Create;

namespace WebApp.Api.V1.Milestones.Create;

public sealed record Request
{
    public ProjectId? ProjectId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? Emoji { get; init; }
    public string? Color { get; init; }
    public MilestoneStatusId? StatusId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.ProjectId).NotNull().WithErrorCode(ErrorCodes.Required);
        RuleFor(a => a.Title).NotEmpty().WithErrorCode(ErrorCodes.Required);
        RuleFor(a => a.Emoji).NotEmpty().WithErrorCode(ErrorCodes.Required);
        RuleFor(a => a.Color).NotEmpty().WithErrorCode(ErrorCodes.Required);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateMilestone ToCommand(this Request request);
}
