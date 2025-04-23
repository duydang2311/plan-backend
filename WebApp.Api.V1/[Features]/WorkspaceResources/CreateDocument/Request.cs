using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.CreateDocument;

namespace WebApp.Api.V1.WorkspaceResources.CreateDocument;

public record Request
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Name { get; init; }
    public string? Content { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId CreatorId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.WorkspaceId).NotNull().WithErrorCode("required");
        RuleFor(a => a.Content).NotEmpty().WithErrorCode("required");
        RuleFor(a => a.Name).NotEmpty().WithErrorCode("required");
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateWorkspaceDocumentResource ToCommand(this Request request);
}
