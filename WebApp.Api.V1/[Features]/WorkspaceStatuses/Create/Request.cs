using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceStatuses.Create;

namespace WebApp.Api.V1.WorkspaceStatuses.Create;

public sealed record Request
{
    public WorkspaceId WorkspaceId { get; init; }
    public string? Value { get; init; }
    public string? Description { get; init; }
    public StatusCategory? Category { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.Value).NotEmpty().WithErrorCode("string");
        RuleFor(a => a.Category).NotNull().WithErrorCode("required").IsInEnum().WithErrorCode("invalid");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateWorkspaceStatus ToCommand(this Request request);
}
