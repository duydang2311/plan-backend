using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Resources.Delete;

public sealed record DeleteResource : ICommand<OneOf<ServerError, NotFoundError, Success>>
{
    public required ResourceId Id { get; init; }
}
