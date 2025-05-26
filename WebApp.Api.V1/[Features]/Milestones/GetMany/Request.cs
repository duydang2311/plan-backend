using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using WebApp.Common.Constants;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Milestones.GetMany;

namespace WebApp.Api.V1.Milestones.GetMany;

public sealed record Request : Collective
{
    public ProjectId? ProjectId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.ProjectId).NotNull().WithErrorCode(ErrorCodes.Required);
    }
}

public static class RequestMapper
{
    public static GetMilestones ToCommand(this Request request)
    {
        return new GetMilestones { ProjectId = request.ProjectId ?? ProjectId.Empty, Select = request.Select };
    }
}
