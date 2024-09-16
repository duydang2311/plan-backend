using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectIssues.GetMany;

namespace WebApp.Api.V1.ProjectIssues.GetMany;

public sealed record Request : Collective
{
    public ProjectId ProjectId { get; init; }
    public string? Select { get; init; }
    public StatusId? StatusId { get; init; }
    public bool? NullStatusId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetProjectIssues ToCommand(this Request request);
}
