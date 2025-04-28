using Riok.Mapperly.Abstractions;
using WebApp.Api.V1.Common.Dtos;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.WorkspaceResources.GetOne;

public sealed record Response : BaseWorkspaceResourceDto { }

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[UseStaticMapper(typeof(DtoMapper))]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this WorkspaceResource workspaceResource);
}
