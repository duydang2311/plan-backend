using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.SoftDeleteOne;

public sealed record SoftDeleteIssue : ICommand<OneOf<ValidationFailures, Success>>
{
    public IssueId IssueId { get; init; }
}
