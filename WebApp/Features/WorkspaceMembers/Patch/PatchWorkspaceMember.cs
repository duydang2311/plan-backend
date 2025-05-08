using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceMembers.Patch;

public sealed record PatchWorkspaceMember
    : ICommand<OneOf<NotFoundError, InvalidPatchError, Success>>
{
    public required WorkspaceMemberId Id { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public RoleId? RoleId { get; init; }
    }
}
