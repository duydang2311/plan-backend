using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Features.Users.CreateWithGoogle;

namespace WebApp.Api.V1.Users.CreateWithGoogle;

public sealed record class Request
{
    public string? IdToken { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial CreateUserWithGoogleCommand ToCommand(this Request request);
}
