using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMembers.Create;

public sealed record CreateProjectMember : ICommand<OneOf<ValidationFailures, ConflictError, Success>>
{
    public required ProjectId ProjectId { get; init; }
    public required UserId UserId { get; init; }
    public required RoleId RoleId { get; init; }
}
