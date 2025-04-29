using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.ResourceFiles.CreateBatch;

namespace WebApp.Api.V1.ResourceFiles.CreateBatch;

public sealed record Request
{
    public IReadOnlyCollection<RequestBatchItem>? Files { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed record RequestBatchItem
{
    public ResourceId? ResourceId { get; init; }
    public string? Key { get; init; }
    public string? OriginalName { get; init; }
    public long? Size { get; init; }
    public string? MimeType { get; init; }
    public StoragePendingUploadId? PendingUploadId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.Files).NotEmpty().WithErrorCode("required");
        RuleForEach(a => a.Files)
            .ChildRules(file =>
            {
                file.RuleFor(a => a.ResourceId).NotNull().WithErrorCode("required");
                file.RuleFor(a => a.Key).NotEmpty().WithErrorCode("required");
                file.RuleFor(a => a.OriginalName).NotEmpty().WithErrorCode("required");
                file.RuleFor(a => a.Size).NotNull().WithErrorCode("required");
                file.RuleFor(a => a.MimeType).NotEmpty().WithErrorCode("required");
            });
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateResourceFileBatch ToCommand(this Request request);
}
