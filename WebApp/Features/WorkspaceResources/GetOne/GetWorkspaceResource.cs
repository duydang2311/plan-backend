using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceResources.GetOne;

public sealed record GetWorkspaceResource : ICommand<OneOf<NotFoundError, WorkspaceResource>>
{
    public required ResourceId Id { get; init; }
    public string? Select { get; init; }
}
