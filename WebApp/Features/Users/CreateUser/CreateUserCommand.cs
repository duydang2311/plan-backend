using FastEndpoints;
using OneOf;
using WebApp.SharedKernel.Models;

namespace WebApp.Features.Users.CreateUser;

public sealed record class CreateUserCommand(string Email, string Password)
    : ICommand<OneOf<IEnumerable<ValidationError>, CreateUserResult>> { }
