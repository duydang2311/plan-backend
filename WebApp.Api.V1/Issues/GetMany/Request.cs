using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.GetMany;

namespace WebApp.Api.V1.Issues.GetMany;

public sealed record Request : Collective
{
    [QueryParam]
    public TeamId? TeamId { get; init; }

    [QueryParam]
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetIssues ToCommand(this Request request);
}
