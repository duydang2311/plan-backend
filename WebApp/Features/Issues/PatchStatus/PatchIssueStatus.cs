using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.PatchStatus;

public sealed record PatchIssueStatus : ICommand<OneOf<ValidationFailures, Success>>
{
    public required IssueId IssueId { get; init; }
    public StatusId StatusId { get; init; }
    public long? OrderByStatus { get; init; }
}
