using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.GetOne;

namespace WebApp.Api.V1.Issues.GetOne.ByOrderNumber;

public sealed record Request
{
    public ProjectId ProjectId { get; init; }
    public long OrderNumber { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.UserId))]
    [MapperIgnoreTarget(nameof(GetIssue.IssueId))]
    public static partial GetIssue ToCommand(this Request request);
}
