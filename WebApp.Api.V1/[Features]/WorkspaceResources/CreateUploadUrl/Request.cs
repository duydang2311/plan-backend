using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.CreateUploadUrls;

namespace WebApp.Api.V1.WorkspaceResources.CreateUploadUrl;

public sealed record Request
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Key { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.WorkspaceId).NotNull().WithErrorCode("required");
        RuleFor(x => x.Key).NotEmpty().WithErrorCode("required").MaximumLength(255).WithErrorCode("max_length");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static CreateWorkspaceResourceUploadUrls ToCommand(this Request request) =>
        new() { WorkspaceId = request.WorkspaceId!.Value, Keys = [request.Key!] };
}
