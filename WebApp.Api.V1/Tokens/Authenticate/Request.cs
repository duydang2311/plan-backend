using FastEndpoints;
using FluentValidation;

namespace WebApp.Api.V1.Tokens.Authenticate;

public sealed record class Request(string? Email, string? Password);

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
