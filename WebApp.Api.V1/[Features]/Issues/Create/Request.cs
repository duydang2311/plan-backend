using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.Create;

namespace WebApp.Api.V1.Issues.Create;

public sealed record Request
{
    public ProjectId? ProjectId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public StatusId? StatusId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId AuthorId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.ProjectId).NotNull();
        RuleFor(x => x.Title).NotEmpty();
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial CreateIssue ToCommand(this Request request);
}
