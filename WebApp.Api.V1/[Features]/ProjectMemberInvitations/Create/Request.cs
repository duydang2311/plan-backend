using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectMemberInvitations.Create;

namespace WebApp.Api.V1.ProjectMemberInvitations.Create;

public sealed record Request
{
    public ProjectId? ProjectId { get; init; }
    public UserId? UserId { get; init; }
    public ProjectRole? Role { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }

    public enum ProjectRole
    {
        Administrator,
        Manager,
        Member,
        Guest
    }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.ProjectId).NotNull().WithErrorCode("string");
        RuleFor(a => a.UserId).NotNull().WithErrorCode("string");
        RuleFor(a => a.Role).NotNull().WithErrorCode("string").IsInEnum().WithErrorCode("invalid");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    [MapProperty(nameof(Request.Role), nameof(CreateProjectMemberInvitation.RoleId))]
    public static partial CreateProjectMemberInvitation ToCommand(this Request request);

    public static RoleId MapRole(this Request.ProjectRole role) =>
        role switch
        {
            Request.ProjectRole.Administrator => ProjectRoleDefaults.Admin.Id,
            Request.ProjectRole.Manager => ProjectRoleDefaults.Manager.Id,
            Request.ProjectRole.Member => ProjectRoleDefaults.Member.Id,
            Request.ProjectRole.Guest => ProjectRoleDefaults.Guest.Id,
            _ => throw new InvalidOperationException()
        };
}
