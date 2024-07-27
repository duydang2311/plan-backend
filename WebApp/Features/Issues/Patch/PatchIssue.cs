using FastEndpoints;
using Json.Patch;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.Patch;

public sealed record PatchIssue : ICommand<OneOf<ValidationFailures, Issue>>
{
    public required IssueId IssueId { get; init; }
    public required JsonPatch Patch { get; init; }
}
