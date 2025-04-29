using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Resources.Patch;

public sealed record PatchResource : ICommand<OneOf<NotFoundError, Success>>
{
    public required ResourceId Id { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string? Name { get; init; }
    }
}
