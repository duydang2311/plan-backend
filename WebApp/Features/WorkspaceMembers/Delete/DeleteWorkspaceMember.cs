using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceMembers.Delete;

public sealed record DeleteWorkspaceMember : ICommand<OneOf<NotFoundError, Success>>
{
    public required WorkspaceMemberId Id { get; init; }
}
