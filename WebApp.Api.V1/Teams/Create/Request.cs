using System.Security.Claims;
using FastEndpoints;
using FluentValidation;

namespace WebApp.Api.V1.Teams.Create;

public sealed record Request
{
    public Guid? WorkspaceId { get; init; }
    public string? Name { get; init; }
    public string? Identifier { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public Guid UserId { get; init; }

    public string? QualifiedName => Name?.Trim();
    public string? QualifiedIdentifier => Identifier?.Trim().ToUpperInvariant();
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.WorkspaceId).NotNull();
        RuleFor(x => x.QualifiedName).NotEmpty().WithErrorCode("required").OverridePropertyName(nameof(Request.Name));
        RuleFor(x => x.QualifiedIdentifier)
            .NotEmpty()
            .WithErrorCode("required")
            .MaximumLength(5)
            .WithErrorCode("maxLength")
            .OverridePropertyName(nameof(Request.Identifier));
    }
}
