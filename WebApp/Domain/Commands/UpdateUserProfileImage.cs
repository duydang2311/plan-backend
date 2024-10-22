using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Commands;

public sealed record UpdateUserProfileImage : ICommand
{
    public required UserId UserId { get; init; }
    public required string PublicId { get; init; }
    public required string ResourceType { get; init; }
    public required string Format { get; init; }
    public required int Version { get; init; }
}
