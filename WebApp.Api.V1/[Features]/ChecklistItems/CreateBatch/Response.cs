using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ChecklistItems.CreateBatch;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[UseStaticMapper(typeof(Create.ResponseMapper))]
public static partial class ResponseMapper
{
    public static partial IReadOnlyCollection<Create.Response> ToResponse(
        this IReadOnlyCollection<ChecklistItem> checklistItems
    );
}
