using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.PatchStatus;

namespace WebApp.Api.V1.Issues.PatchStatus;

public sealed record Request
{
    public IssueId IssueId { get; init; }
    public StatusId? StatusId { get; init; }
    public long? OrderByStatus { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.StatusId).NotNull();
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial PatchIssueStatus ToCommand(this Request request);
}
