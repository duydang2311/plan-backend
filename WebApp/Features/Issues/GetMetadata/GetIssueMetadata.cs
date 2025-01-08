using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.GetMetadata;

public sealed record GetIssueMetadata : Collective, ICommand<GetIssueMetadataResult>
{
    public UserId UserId { get; init; }
    public TeamId? TeamId { get; init; }
    public ProjectId? ProjectId { get; init; }
    public StatusId? StatusId { get; init; }
    public bool? NullStatusId { get; init; }
}
