using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.IssueComments.SoftDeleteOne;

namespace WebApp.Api.V1.IssueComments.DeleteOne;

public sealed record Request
{
    public IssueCommentId IssueCommentId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial SoftDeleteIssueComment ToCommand(this Request request);
}
