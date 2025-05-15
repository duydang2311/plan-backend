using Riok.Mapperly.Abstractions;
using WebApp.Api.V1.Common.Dtos;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ChecklistItems.GetMany;

public sealed record Response : PaginatedList<BaseChecklistItemDto> { }

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[UseStaticMapper(typeof(DtoMapper))]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<ChecklistItem> list);
}
