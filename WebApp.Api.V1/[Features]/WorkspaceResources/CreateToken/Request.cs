using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.WorkspaceResources.CreateToken;

public sealed record Request
{
    public WorkspaceId? WorkspaceId { get; init; }
    public ResourceFileId? ResourceFileId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.WorkspaceId).NotNull().WithErrorCode("required");
        RuleFor(a => a.ResourceFileId).NotNull().WithErrorCode("required");
    }
}
