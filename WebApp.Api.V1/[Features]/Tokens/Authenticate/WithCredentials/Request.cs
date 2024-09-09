using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Features.Tokens.Authenticate.WithCredentials;

namespace WebApp.Api.V1.Tokens.Authenticate.WithCredentials;

public sealed record class Request(string? Email, string? Password);

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial AuthenticateWithCredentialsCommand ToCommand(this Request request);
}
