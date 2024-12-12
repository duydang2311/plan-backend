using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Users.GetMany;

public sealed record GetUsers : Collective, ICommand<OneOf<PaginatedList<User>>>
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Select { get; init; }
}
