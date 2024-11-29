using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.GetOne;

namespace WebApp.Api.V1.Issues.GetOne.ById;

public sealed record Request
{
    public IssueId IssueId { get; init; }

    [QueryParam]
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.UserId))]
    [MapperIgnoreTarget(nameof(GetIssue.ProjectId))]
    [MapperIgnoreTarget(nameof(GetIssue.OrderNumber))]
    public static partial GetIssue ToCommand(this Request request);
}
