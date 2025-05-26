using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
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

public static class RequestMapper
{
    public static CreateMilestone ToCommand(this Request request)
    {
        return new CreateMilestone
        {
            ProjectId = request.ProjectId ?? ProjectId.Empty,
            Title = request.Title ?? string.Empty,
            Description = request.Description,
            Emoji = request.Emoji ?? string.Empty,
            Color = request.Color ?? string.Empty,
        };
    }
}
