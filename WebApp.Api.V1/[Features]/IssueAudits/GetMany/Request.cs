using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.IssueAudits.GetMany;

namespace WebApp.Api.V1.IssueAudits.GetMany;

public sealed record Request : Collective, IKeysetPagination<long?>
{
    public IssueId? IssueId { get; init; }
    public string? Select { get; init; }
    public long? Cursor { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.RequestingUserId))]
    public static partial GetIssueAudits ToCommand(this Request request);
}
