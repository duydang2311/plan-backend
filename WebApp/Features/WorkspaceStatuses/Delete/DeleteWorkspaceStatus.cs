using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceStatuses.Delete;

public sealed record DeleteWorkspaceStatus : ICommand<OneOf<NotFoundError, Success>>
{
    public required StatusId StatusId { get; init; }
}
