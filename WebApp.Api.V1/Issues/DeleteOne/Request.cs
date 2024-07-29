using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.SoftDeleteOne;

namespace WebApp.Api.V1.Issues.DeleteOne;

public sealed record Request
{
    public IssueId IssueId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial SoftDeleteIssue ToCommand(this Request request);
}
