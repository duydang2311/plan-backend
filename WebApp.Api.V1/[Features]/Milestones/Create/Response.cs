using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Milestones.Create;

public sealed record Response
{
    public required MilestoneId Id { get; init; }
}

public static class ResponseMapper
{
    public static Response ToResponse(this Milestone milestone)
    {
        return new Response { Id = milestone.Id };
    }
}
