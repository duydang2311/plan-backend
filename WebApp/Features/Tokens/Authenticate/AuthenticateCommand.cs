using FastEndpoints;
using OneOf;
using WebApp.SharedKernel.Models;

namespace WebApp.Features.Tokens.Authenticate;

public sealed record class AuthenticateCommand(string Email, string Password)
    : ICommand<OneOf<IEnumerable<ValidationError>, AuthenticateResult>> { }
