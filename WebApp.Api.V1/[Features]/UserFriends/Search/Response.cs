using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.UserFriends.Search;

public sealed record Response : PaginatedList<GetMany.Response.Item> { }

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[UseStaticMapper(typeof(GetMany.ResponseMapper))]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<UserFriend> result);
}
