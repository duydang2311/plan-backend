using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Projects.Create;

public sealed record CreateProject : ICommand<OneOf<ConflictError, ValidationFailures, ServerError, Project>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required string Name { get; init; }
    public required string Identifier { get; init; }
    public required UserId UserId { get; init; }
    public required string? Description { get; init; }
}
