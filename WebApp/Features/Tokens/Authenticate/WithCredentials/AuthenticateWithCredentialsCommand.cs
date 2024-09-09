using FastEndpoints;
using OneOf;
using WebApp.Common.Models;

namespace WebApp.Features.Tokens.Authenticate.WithCredentials;

public sealed record class AuthenticateWithCredentialsCommand(string Email, string Password)
    : ICommand<OneOf<ValidationFailures, AuthenticateResult>> { }
