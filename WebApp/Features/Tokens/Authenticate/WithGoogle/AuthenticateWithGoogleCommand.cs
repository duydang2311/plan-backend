using FastEndpoints;
using OneOf;
using WebApp.Common.Models;

namespace WebApp.Features.Tokens.Authenticate.WithGoogle;

public sealed record class AuthenticateWithGoogleCommand(string IdToken)
    : ICommand<OneOf<ValidationFailures, AuthenticateResult>> { }
