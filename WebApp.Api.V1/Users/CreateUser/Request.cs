using FastEndpoints;
using FluentValidation;

namespace WebApp.Api.V1.Users.CreateUser;

public sealed record class Request
{
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? VerificationUrl { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(7);
    }
}
