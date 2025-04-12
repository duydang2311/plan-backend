using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.Patch;

namespace WebApp.Api.V1.Issues.Patch;

public sealed record Request
{
    public IssueId IssueId { get; init; }

    public Patchable Patch { get; init; } = null!;

    public sealed record Patchable : Patchable<Patchable>
    {
        public string? Title { get; init; }
        public string Description { get; init; } = string.Empty;
        public IssuePriority Priority { get; init; }
        public StatusId StatusId { get; init; }
        public string? StatusRank { get; init; }
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
    [MapperIgnoreSource(nameof(Request.UserId))]
    public static partial PatchIssue ToCommand(this Request request);
}
