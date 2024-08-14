using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Workspaces.HasWorkspace;

public sealed record class Request
{
    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }

    [QueryParam]
    public WorkspaceId? Id { get; init; }

    [QueryParam]
    public string? Path { get; init; }
}

public sealed partial class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Id).NotNull().When(x => string.IsNullOrEmpty(x.Path));
        RuleFor(x => x.Path).NotNull().When(x => x.Id is null);
    }
}
