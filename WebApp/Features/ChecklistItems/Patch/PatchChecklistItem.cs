using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ChecklistItems.Patch;

public sealed record PatchChecklistItem : ICommand<OneOf<InvalidPatchError, NotFoundError, Success>>
{
    public required ChecklistItemId Id { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string? Content { get; init; }
        public bool? Completed { get; init; }
    }
}
