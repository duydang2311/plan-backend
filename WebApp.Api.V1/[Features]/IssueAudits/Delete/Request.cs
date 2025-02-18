using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.IssueAudits.Delete;

namespace WebApp.Api.V1.IssueAudits.Delete;

public sealed record Request
{
    public long Id { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial DeleteIssueAudit ToCommand(this Request request);
}
