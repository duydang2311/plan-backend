using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserProfiles.Patch;

public sealed record PatchUserProfile : ICommand<OneOf<NotFoundError, InvalidPatchError, Success>>
{
    public required UserId UserId { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string? DisplayName { get; init; }
        public string? Bio { get; init; }
    }
}
