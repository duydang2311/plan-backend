using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Projects.Delete;

public sealed record DeleteProject : ICommand<OneOf<NotFoundError, Success>>
{
    public required ProjectId ProjectId { get; init; }
}
