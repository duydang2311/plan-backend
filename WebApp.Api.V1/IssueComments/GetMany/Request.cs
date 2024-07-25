using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.IssueComments.GetMany;

namespace WebApp.Api.V1.IssueComments.GetMany;

public sealed record Request : Collective
{
    public IssueId IssueId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetIssueComments ToCommand(this Request request);
}
