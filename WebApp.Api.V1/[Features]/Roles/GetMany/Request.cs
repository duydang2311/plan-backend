using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Features.Roles.GetMany;

namespace WebApp.Api.V1.Roles.GetMany;

public sealed record Request : Collective
{
    public string? Type { get; init; }
    public string? Select { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetRoles ToCommand(this Request request);
}
