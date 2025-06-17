using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.UserNotifications.GetMany;

namespace WebApp.Api.V1.UserNotifications.GetMany;

public sealed record Request : Collective, IKeysetPagination<UserNotificationId?>
{
    public UserId UserId { get; init; }
    public UserNotificationId? Cursor { get; init; }
    public string? Select { get; init; }
    public string? SelectProject { get; init; }
    public string? SelectIssue { get; init; }
    public string? SelectComment { get; init; }
    public string? SelectProjectMemberInvitation { get; init; }
    public string? SelectWorkspaceInvitation { get; init; }
    public string? SelectStatus { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetUserNotifications ToCommand(this Request request);
}
