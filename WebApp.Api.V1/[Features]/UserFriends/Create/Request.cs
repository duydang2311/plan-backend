using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.UserFriends.Create;

namespace WebApp.Api.V1.UserFriends.Create;

public sealed record Request
{
    public UserId? UserId { get; init; }
    public UserId? FriendId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.UserId).NotNull().WithErrorCode("required");
        RuleFor(a => a.FriendId).NotNull().WithErrorCode("required");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateUserFriend ToCommand(this Request request);
}
