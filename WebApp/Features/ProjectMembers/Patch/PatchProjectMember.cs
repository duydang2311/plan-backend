using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMembers.Patch;

public sealed record PatchProjectMember : ICommand<OneOf<ValidationFailures, NotFoundError, Success>>
{
    public required ProjectMemberId ProjectMemberId { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public RoleId? RoleId { get; init; }
    }
}
