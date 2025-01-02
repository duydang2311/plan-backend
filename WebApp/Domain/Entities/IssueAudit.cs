using System.Text.Json;
using NodaTime;
using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

public sealed record IssueAudit
{
    public long Id { get; init; }
    public Instant CreatedTime { get; init; }
    public IssueId IssueId { get; init; }
    public Issue Issue { get; init; } = null!;
    public IssueAuditAction Action { get; init; }
    public UserId? UserId { get; init; }
    public User? User { get; init; }
    public JsonDocument? Data { get; init; }
}
