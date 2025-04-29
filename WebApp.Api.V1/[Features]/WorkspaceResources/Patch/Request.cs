using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using OneOf;
using OneOf.Types;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Resources.Patch;

namespace WebApp.Api.V1.WorkspaceResources.Patch;

public sealed record Request : ICommand<OneOf<NotFoundError, Success>>
{
    public ResourceId Id { get; init; }
    public Patchable? Patch { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public string? Name { get; init; }
    }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.Patch).NotNull().WithErrorCode("required");
        When(
            a => a.Patch is not null,
            () =>
            {
                RuleFor(a => a.Patch!.Name)
                    .NotEmpty()
                    .WithErrorCode("required")
                    .MaximumLength(255)
                    .WithErrorCode("max_length");
            }
        );
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial PatchResource ToCommand(this Request request);
}
