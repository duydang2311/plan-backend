using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.TeamIssues.Create;

public sealed record CreateTeamIssue : ICommand<OneOf<ValidationFailures, Success>>
{
    public required IssueId IssueId { get; init; }
    public required TeamId TeamId { get; init; }
}
