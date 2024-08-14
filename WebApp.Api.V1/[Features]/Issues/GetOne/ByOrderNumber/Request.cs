using FastEndpoints;
using Riok.Mapperly.Abstractions;
using System.Security.Claims;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.GetOne;

namespace WebApp.Api.V1.Issues.GetOne.ByOrderNumber;

public sealed record Request
{
    public TeamId TeamId { get; init; }
    public long OrderNumber { get; init; }

    [QueryParam]
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetIssue ToCommand(this Request request);
}
