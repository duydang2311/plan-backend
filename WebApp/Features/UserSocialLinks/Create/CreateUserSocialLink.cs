using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserSocialLinks.Create;

public sealed record CreateUserSocialLink : ICommand<OneOf<NotFoundError, UserSocialLink>>
{
    public required UserId UserId { get; init; }
    public required string Url { get; init; }
}
