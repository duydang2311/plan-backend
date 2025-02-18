using System.Security.Claims;
using System.Text.Json;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.IssueAudits.Patch;

namespace WebApp.Api.V1.IssueAudits.PatchComment;

public sealed record Request
{
    public long Id { get; init; }
    public Patchable Patch { get; init; } = null!;

    public sealed record Patchable : Patchable<Patchable>
    {
        public JsonDocument? Data { get; init; }
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

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial PatchIssueAudit ToCommand(this Request request);
}
