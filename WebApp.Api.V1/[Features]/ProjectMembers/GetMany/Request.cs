using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectMembers.GetMany;

namespace WebApp.Api.V1.ProjectMembers.GetMany;

public sealed record Request : Collective
{
    public ProjectId? ProjectId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.RequestingUserId))]
    public static partial GetProjectMembers ToCommand(this Request request);
}
