using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.UserProfiles.Create;

namespace WebApp.Api.V1.UserProfiles.Create;

public sealed record Request
{
    public UserId UserId { get; init; }
    public string? Name { get; init; }
    public string? DisplayName { get; init; }
    public Asset? Image { get; init; }
    public string? Bio { get; init; }
    public ICollection<string>? SocialLinks { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.Name).NotEmpty().WithErrorCode("string");
        RuleFor(a => a.DisplayName).NotEmpty().WithErrorCode("string");
        When(
            a => a.Image is not null,
            () =>
            {
                RuleFor(a => a.Image!.PublicId).NotEmpty().WithErrorCode("string");
                RuleFor(a => a.Image!.ResourceType).NotEmpty().WithErrorCode("string");
                RuleFor(a => a.Image!.Format).NotEmpty().WithErrorCode("string");
                RuleFor(a => a.Image!.Version).NotNull().WithErrorCode("number");
            }
        );
        RuleFor(a => a.Bio).NotEmpty().When(a => a.Bio is not null);
        When(
            a => a.SocialLinks is not null,
            () =>
            {
                RuleFor(a => a.SocialLinks).NotEmpty().WithErrorCode("array");
                RuleForEach(a => a.SocialLinks).NotEmpty().WithErrorCode("string");
            }
        );
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateUserProfile ToCommand(this Request request);
}
