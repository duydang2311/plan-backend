using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.TeamIssues.Delete;

public sealed record DeleteTeamIssue : ICommand<OneOf<NotFoundError, Success>>
{
    public required TeamId TeamId { get; init; }
    public required IssueId IssueId { get; init; }
}
