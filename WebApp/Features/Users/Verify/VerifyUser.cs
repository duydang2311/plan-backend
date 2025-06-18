using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;

namespace WebApp.Features.Users.Verify;

public sealed record VerifyUser : ICommand<OneOf<NotFoundError, Success>>
{
    public required Guid Token { get; init; }
}
