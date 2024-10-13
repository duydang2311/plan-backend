using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Users.GetOne;

public sealed record GetUser : ICommand<OneOf<ValidationFailures, NotFoundError, User>>
{
    public UserId? UserId { get; init; }
    public string? ProfileName { get; init; }

    public string? Select { get; init; }
}
