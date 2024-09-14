using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectStatuses.GetMany;

namespace WebApp.Api.V1.ProjectStatuses.GetMany;

public sealed record Request : Collective
{
    public ProjectId ProjectId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetProjectStatuses ToCommand(this Request request);
}
