using FastEndpoints;
using OneOf;

namespace WebApp.Features.Tokens.Authenticate;

public sealed record class AuthenticateCommand(string Email, string Password) : ICommand<OneOf<ProblemDetails, AuthenticateResult>> { }
