using Riok.Mapperly.Abstractions;
using WebApp.Features.Users.Verify;

namespace WebApp.Api.V1.Users.Verify;

public sealed record Request
{
    public Guid Token { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial VerifyUser ToCommand(this Request request);
}
