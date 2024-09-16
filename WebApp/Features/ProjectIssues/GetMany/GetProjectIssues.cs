using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectIssues.GetMany;

public sealed record GetProjectIssues : Collective, ICommand<OneOf<PaginatedList<ProjectIssue>>>
{
    public required UserId UserId { get; init; }
    public required ProjectId ProjectId { get; init; }
    public string? Select { get; init; }
    public StatusId? StatusId { get; init; }
    public bool? NullStatusId { get; init; }
}
