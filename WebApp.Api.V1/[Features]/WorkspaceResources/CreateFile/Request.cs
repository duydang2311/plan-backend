using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.CreateFile;

namespace WebApp.Api.V1.WorkspaceResources.CreateFile;

public record Request
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Key { get; init; }
    public StoragePendingUploadId? PendingUploadId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId CreatorId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.WorkspaceId).NotNull().WithErrorCode("required");
        RuleFor(a => a.Key).NotEmpty().WithErrorCode("required");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateWorkspaceFileResource ToCommand(this Request request);
}
