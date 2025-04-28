using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceResources.Delete;

public sealed record DeleteWorkspaceResource : ICommand<OneOf<NotFoundError, Success>>
{
    public required ResourceId Id { get; init; }
}
