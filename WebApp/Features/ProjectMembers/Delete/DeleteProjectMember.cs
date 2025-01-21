using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMembers.Delete;

public sealed record DeleteProjectMember : ICommand<OneOf<NotFoundError, Success>>
{
    public required ProjectMemberId Id { get; init; }
}
