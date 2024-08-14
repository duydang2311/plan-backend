using FastEndpoints;
using FluentValidation;

namespace WebApp.Api.V1.Tokens.Refresh;

public sealed record class Request(Guid? RefreshToken);

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotNull();
    }
}
