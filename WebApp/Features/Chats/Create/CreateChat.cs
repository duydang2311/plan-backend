using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Chats.Create;

public sealed record CreateChat : ICommand<OneOf<ValidationFailures, Success>>
{
    public UserId OwnerId { get; init; }
    public ICollection<UserId> MemberIds { get; init; } = Array.Empty<UserId>();
}
