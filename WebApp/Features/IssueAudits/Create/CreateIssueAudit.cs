using System.Text.Json;
using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueAudits.Create;

public sealed record CreateIssueAudit : ICommand<OneOf<ValidationFailures, IssueAudit>>
{
    public required IssueId IssueId { get; init; }
    public required IssueAuditAction Action { get; init; }
    public UserId? UserId { get; init; }
    public JsonDocument? Data { get; init; }
}
