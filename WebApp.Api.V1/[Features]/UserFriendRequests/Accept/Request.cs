using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.UserFriendRequests.Accept;

namespace WebApp.Api.V1.UserFriendRequests.Accept;

public sealed record Request
{
    public UserId? SenderId { get; init; }
    public UserId? ReceiverId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.SenderId).NotNull().WithErrorCode("required");
        RuleFor(a => a.ReceiverId).NotNull().WithErrorCode("required");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial AcceptUserFriendRequest ToCommand(this Request request);
}
