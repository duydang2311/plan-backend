using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Issues.GetMetadata;

namespace WebApp.Api.V1.Issues.GetMetadata;

public sealed record Request : Collective
{
    public TeamId? TeamId { get; init; }
    public ProjectId? ProjectId { get; init; }
    public StatusId? StatusId { get; init; }
    public bool? NullStatusId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetIssueMetadata ToCommand(this Request request);
}
