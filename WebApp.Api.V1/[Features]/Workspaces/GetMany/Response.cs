using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Workspaces.GetMany;

public sealed record Response : PaginatedList<Get.Response> { }

[Mapper]
[UseStaticMapper(typeof(Get.ResponseMapper))]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<Workspace> list);
}
