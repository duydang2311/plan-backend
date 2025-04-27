using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Tokens.Authenticate;

namespace WebApp.Api.V1.Tokens.Authenticate;

public sealed record class Response(SessionId SessionId, int SessionMaxAge);

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this AuthenticateResult result);
}
