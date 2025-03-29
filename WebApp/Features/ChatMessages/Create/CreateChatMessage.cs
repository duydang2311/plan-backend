using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ChatMessages.Create;

public sealed record CreateChatMessage : ICommand<OneOf<ValidationFailures, Success>>
{
    public required ChatId ChatId { get; init; }
    public required UserId SenderId { get; init; }
    public required string Content { get; init; }
}
