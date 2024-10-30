using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Issues.GetMany;

public sealed record Response : PaginatedList<GetOne.Response> { }

[Mapper]
[UseStaticMapper(typeof(GetOne.ResponseMapper))]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<Issue> issue);
}
