using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.IssueComments.GetOne;

namespace WebApp.Api.V1.IssueComments.GetOne;

public sealed record Request
{
    public IssueCommentId IssueCommentId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetIssueComment ToCommand(this Request request);
}
