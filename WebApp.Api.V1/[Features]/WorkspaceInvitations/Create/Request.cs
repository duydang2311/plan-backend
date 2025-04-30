using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceInvitations.Create;

namespace WebApp.Api.V1.WorkspaceInvitations.Create;

public sealed record Request
{
    public WorkspaceId WorkspaceId { get; init; }
    public UserId? UserId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.UserId).NotNull().WithErrorCode("required");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateWorkspaceInvitation ToCommand(this Request request);
}
