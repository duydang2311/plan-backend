using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Projects.Create;

public sealed record Request
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Name { get; init; }
    public string? Identifier { get; init; }
    public string? Description { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.WorkspaceId).NotNull();
        RuleFor(a => a.Name).NotNull();
        RuleFor(a => a.Identifier).NotNull();
    }
}
