using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserFriendRequests.Accept;

public sealed record AcceptUserFriendRequest : ICommand<OneOf<NotFoundError, ConflictError, Success>>
{
    public UserId SenderId { get; init; }
    public UserId ReceiverId { get; init; }
}
