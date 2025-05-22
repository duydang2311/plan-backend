using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;

namespace WebApp.Features.UserSocialLinks.Delete;

public sealed record DeleteUserSocialLink : ICommand<OneOf<NotFoundError, Success>>
{
    public required long Id { get; init; }
}
