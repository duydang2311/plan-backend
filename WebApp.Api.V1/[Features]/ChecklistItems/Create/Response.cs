using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ChecklistItems.Create;

public sealed record Response
{
    public required ChecklistItemId Id { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this ChecklistItem checklistItem);
}
