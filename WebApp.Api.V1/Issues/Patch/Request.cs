using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Json.Patch;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.Patch;

namespace WebApp.Api.V1.Issues.Patch;

public sealed record Request
{
    public IssueId IssueId { get; init; }
    public JsonPatch? Patch { get; init; } = null!;

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Patch).NotNull();
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial PatchIssue ToCommand(this Request request);
}
