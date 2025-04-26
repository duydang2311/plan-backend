using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.Create;

namespace WebApp.Api.V1.WorkspaceResources.Create;

public record Request
{
    public WorkspaceId? WorkspaceId { get; init; }
    public StoragePendingUploadId? PendingUploadId { get; init; }
    public string? Name { get; init; }
    public string? Content { get; init; }
    public ICollection<RequestResourceFile>? Files { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId CreatorId { get; init; }
}

public sealed record RequestResourceFile
{
    public string? Key { get; init; }
    public string? OriginalName { get; init; }
    public StoragePendingUploadId? PendingUploadId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.WorkspaceId).NotNull().WithErrorCode("required");
        RuleFor(a => a.Name).NotEmpty().WithErrorCode("required");
        When(
            a => a.Files is not null,
            () =>
            {
                RuleForEach(a => a.Files)
                    .ChildRules(file =>
                    {
                        file.RuleFor(a => a.Key).NotEmpty().WithErrorCode("required");
                        file.RuleFor(a => a.OriginalName).NotEmpty().WithErrorCode("required");
                    });
            }
        );
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateWorkspaceResource ToCommand(this Request request);
}
