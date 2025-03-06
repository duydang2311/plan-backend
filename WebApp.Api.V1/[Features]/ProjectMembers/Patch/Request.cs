using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectMembers.Patch;

namespace WebApp.Api.V1.ProjectMembers.Patch;

public sealed record Request
{
    public ProjectMemberId ProjectMemberId { get; init; }
    public Patchable? Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public RoleId? RoleId { get; init; }
    }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.Patch).NotNull().WithErrorCode("required");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial PatchProjectMember ToCommand(this Request request);
}
