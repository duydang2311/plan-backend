using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMemberInvitations.GetMany;

public sealed record GetProjectMemberInvitations : Collective, ICommand<PaginatedList<ProjectMemberInvitation>>
{
    public required ProjectId ProjectId { get; init; }
    public string? Select { get; init; }
}
