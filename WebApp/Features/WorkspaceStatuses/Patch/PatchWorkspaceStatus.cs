using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceStatuses.Patch;

public sealed record PatchWorkspaceStatus : ICommand<OneOf<ValidationFailures, NotFoundError, Success>>
{
    public required StatusId StatusId { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public int Rank { get; init; }
    }
}
