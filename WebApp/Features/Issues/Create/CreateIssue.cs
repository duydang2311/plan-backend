using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.Create;

public sealed record CreateIssue : ICommand<OneOf<ValidationFailures, Issue>>
{
    public required UserId AuthorId { get; init; }
    public required ProjectId ProjectId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public StatusId? StatusId { get; init; }
}
