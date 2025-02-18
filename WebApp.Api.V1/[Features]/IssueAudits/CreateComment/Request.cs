using System.Security.Claims;
using System.Text.Json;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.IssueAudits.Create;

namespace WebApp.Api.V1.IssueAudits.CreateComment;

public sealed record Request
{
    public IssueId IssueId { get; init; }
    public UserId? UserId { get; init; }
    public JsonDocument? Data { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateIssueAudit ToCommand(this Request request, IssueAuditAction action);

    public static CreateIssueAudit ToCommand(this Request request) => request.ToCommand(IssueAuditAction.Comment);
}
