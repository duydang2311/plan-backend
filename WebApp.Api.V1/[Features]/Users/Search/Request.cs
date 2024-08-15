using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Features.Users.Search;

namespace WebApp.Api.V1.Users.Search;

public sealed record Request : Collective
{
    public string? Query { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Query).NotNull();
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial SearchUsers ToCommand(this Request request);
}
