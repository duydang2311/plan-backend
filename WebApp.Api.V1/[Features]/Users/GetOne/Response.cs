using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Api.V1.Common.Dtos;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Users.GetOne;

public sealed record Response : BaseUserDto { }

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
[UseStaticMapper(typeof(DtoMapper))]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this User user);
}
