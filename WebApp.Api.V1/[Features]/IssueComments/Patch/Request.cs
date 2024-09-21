using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Json.Patch;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.IssueComments.Patch;

namespace WebApp.Api.V1.IssueComments.Patch;

public sealed record Request
{
    public IssueCommentId IssueCommentId { get; init; }
    public Patchable Patch { get; init; } = null!;

    public sealed record Patchable : Patchable<Patchable>
    {
        public string Content { get; init; } = string.Empty;
    }

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
    public static partial PatchIssueComment ToCommand(this Request request);
}
