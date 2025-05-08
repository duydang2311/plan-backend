using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceMembers.Patch;

namespace WebApp.Api.V1.WorkspaceMembers.Patch;

public sealed record Request
{
    public WorkspaceMemberId Id { get; init; }
    public Patchable? Patch { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public RoleId? RoleId { get; init; }
    }
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
    public static partial PatchWorkspaceMember ToCommand(this Request request);
}
