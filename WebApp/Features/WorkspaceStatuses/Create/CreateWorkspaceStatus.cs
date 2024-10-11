using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceStatuses.Create;

public sealed record CreateWorkspaceStatus : ICommand<OneOf<ValidationFailures, WorkspaceStatus>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required string Value { get; init; }
    public string? Description { get; init; }
}
