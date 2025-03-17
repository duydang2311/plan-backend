using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserFriendRequests.Create;

public sealed record CreateUserFriendRequest : ICommand<OneOf<ValidationFailures, DuplicatedError, Success>>
{
    public required UserId SenderId { get; init; }
    public required UserId ReceiverId { get; init; }
}
