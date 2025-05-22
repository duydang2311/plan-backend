using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.UserSocialLinks.Create;

namespace WebApp.Api.V1.UserSocialLinks.Create;

public sealed record Request
{
    public UserId? UserId { get; init; }
    public string? Url { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.UserId).NotNull().WithErrorCode(ErrorCodes.Required);
        RuleFor(a => a.Url)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Required)
            .Must(a =>
                Uri.IsWellFormedUriString(a, UriKind.Absolute)
                && new Uri(a).Scheme.EqualsEither(
                    [Uri.UriSchemeHttp, Uri.UriSchemeHttps, Uri.UriSchemeMailto],
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .WithErrorCode(ErrorCodes.InvalidValue);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateUserSocialLink ToCommand(this Request request);
}
