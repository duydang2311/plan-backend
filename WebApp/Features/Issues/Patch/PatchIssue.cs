using FastEndpoints;
using Json.Patch;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.Patch;

public sealed record PatchIssue : ICommand<OneOf<ValidationFailures, Success>>
{
    public required IssueId IssueId { get; init; }
    public required JsonPatch Patch { get; init; }
}
