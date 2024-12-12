using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.IssueAssignees.Delete;

namespace WebApp.Api.V1.IssueAssignees.Delete;

public sealed record Request
{
    public IssueId IssueId { get; init; }
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial DeleteIssueAssignee ToCommand(this Request request);
}
