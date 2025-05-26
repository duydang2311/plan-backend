using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Milestones.Create;

public sealed record CreateMilestone : ICommand<OneOf<NotFoundError, Milestone>>
{
    public required ProjectId ProjectId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string Emoji { get; init; }
    public required string Color { get; init; }
}
