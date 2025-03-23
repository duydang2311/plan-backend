using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.UserFriends.Search;

namespace WebApp.Api.V1.UserFriends.Search;

public sealed record Request : Collective
{
    public UserId UserId { get; init; }
    public string? Query { get; init; }
    public string? Select { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Query).NotNull().WithErrorCode("required");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial SearchUserFriends ToCommand(this Request request);
}
