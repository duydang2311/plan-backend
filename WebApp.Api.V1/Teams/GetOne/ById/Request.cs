using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using WebApp.SharedKernel.Models;

namespace WebApp.Api.V1.Teams.GetOne.ById;

public sealed record Request
{
    public TeamId Id { get; init; }

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public Guid UserId { get; init; }
}
