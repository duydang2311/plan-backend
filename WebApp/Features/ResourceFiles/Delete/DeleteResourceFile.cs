using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ResourceFiles.Delete;

public sealed record DeleteResourceFile : ICommand<OneOf<ServerError, NotFoundError, Success>>
{
    public required ResourceFileId Id { get; init; }
}
