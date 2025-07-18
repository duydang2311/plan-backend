using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Projects.Search;

namespace WebApp.Api.V1.Projects.Search;

public sealed record Request : Collective
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Query { get; init; }
    public string? Select { get; init; }
    public float? Threshold { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.WorkspaceId).NotNull().WithErrorCode(ErrorCodes.Required);
        RuleFor(x => x.Query).NotEmpty().WithErrorCode(ErrorCodes.Required);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial SearchProjects ToCommand(this Request request);
}
