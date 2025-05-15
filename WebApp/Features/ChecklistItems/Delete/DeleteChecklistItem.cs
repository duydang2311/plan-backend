using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ChecklistItems.Delete;

public sealed record DeleteChecklistItem : ICommand<OneOf<NotFoundError, Success>>
{
    public required ChecklistItemId Id { get; init; }
}
