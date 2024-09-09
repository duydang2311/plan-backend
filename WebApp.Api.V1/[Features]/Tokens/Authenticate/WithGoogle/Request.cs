using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Features.Tokens.Authenticate.WithGoogle;

namespace WebApp.Api.V1.Tokens.Authenticate.WithGoogle;

public sealed record class Request(string? IdToken);

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
    public static partial AuthenticateWithGoogleCommand ToCommand(this Request request);
}
