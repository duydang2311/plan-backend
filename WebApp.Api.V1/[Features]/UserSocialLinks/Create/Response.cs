using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.UserSocialLinks.Create;

public sealed record Response
{
    public long Id { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this UserSocialLink userSocialLink);
}
