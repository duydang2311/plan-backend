using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ProjectMemberInvitations.GetMany;

public sealed record Response : PaginatedList<GetOne.Response> { }

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[UseStaticMapper(typeof(GetOne.ResponseMapper))]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<ProjectMemberInvitation> list);
}
