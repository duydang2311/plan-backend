using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.UserProfiles.Patch;

namespace WebApp.Api.V1.UserProfiles.Patch;

public sealed record Request
{
    public UserId UserId { get; init; }
    public Patchable? Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string? DisplayName { get; init; }
        public string? Bio { get; init; }
    }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.Patch).NotNull().WithErrorCode(ErrorCodes.Required);
        RuleFor(a => a.Patch)
            .Must(a => a is not null && (a.DisplayName is not null || a.Bio is not null))
            .WithErrorCode(ErrorCodes.InvalidValue);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial PatchUserProfile ToCommand(this Request request);
}
