using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Roles.GetMany;

public sealed record GetRoles : Collective, ICommand<PaginatedList<Role>>
{
    public string? Type { get; init; }
    public string? Select { get; init; }
}
