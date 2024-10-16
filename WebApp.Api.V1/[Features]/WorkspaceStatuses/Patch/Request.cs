using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceStatuses.Patch;

namespace WebApp.Api.V1.WorkspaceStatuses.Patch;

public sealed record Request
{
    public StatusId StatusId { get; init; }
    public Patchable? Patch { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public int Rank { get; init; }
    }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.Patch).NotNull().WithErrorCode("object");
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial PatchWorkspaceStatus ToCommand(this Request request);
}
