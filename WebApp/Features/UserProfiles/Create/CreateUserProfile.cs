using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserProfiles.Create;

public sealed record CreateUserProfile : ICommand<OneOf<NotFoundError, DuplicatedError, ValidationFailures, Success>>
{
    public required UserId UserId { get; init; }
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public Asset? Image { get; init; }
    public string? Bio { get; init; }
    public ICollection<string>? SocialLinks { get; init; }
}
