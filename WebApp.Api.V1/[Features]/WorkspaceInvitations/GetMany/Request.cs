using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceInvitations.GetMany;

namespace WebApp.Api.V1.WorkspaceInvitations.GetMany;

public sealed record Request : Collective
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.WorkspaceId).NotNull().WithErrorCode("required");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetWorkspaceInvitations ToCommand(this Request request);
}
