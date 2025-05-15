using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.ChecklistItems.Patch;

namespace WebApp.Api.V1.ChecklistItems.Patch;

public sealed record Request
{
    public ChecklistItemId Id { get; init; }
    public Patchable? Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string? Content { get; init; }
        public bool? Completed { get; init; }
    }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Patch).NotNull().WithErrorCode(ErrorCodes.Required);
        When(
            a => a.Patch is not null,
            () =>
            {
                RuleFor(a => a.Patch)
                    .Must(a => !string.IsNullOrEmpty(a!.Content) || a.Completed.HasValue)
                    .WithErrorCode(ErrorCodes.Required);
            }
        );
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial PatchChecklistItem ToCommand(this Request request);
}
