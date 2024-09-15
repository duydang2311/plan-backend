using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Projects.Create;

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
        RuleFor(a => a.Name).NotEmpty();
        RuleFor(a => a.Identifier).NotEmpty();
    }
}

[Mapper]
public static partial class RequestMapepr
{
    public static partial CreateProject ToCommand(this Request request);
}
