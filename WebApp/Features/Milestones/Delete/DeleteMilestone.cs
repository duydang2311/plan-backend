using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Milestones.Delete;

public sealed record DeleteMilestone : ICommand<OneOf<NotFoundError, Success>>
{
    public required MilestoneId Id { get; init; }
}
