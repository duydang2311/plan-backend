using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.ChecklistItems.Create;

namespace WebApp.Api.V1.ChecklistItems.CreateBatch;

public sealed record Request
{
    public IssueId? ParentIssueId { get; init; }
    public string[]? Contents { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.ParentIssueId).NotNull().WithErrorCode(ErrorCodes.Required);
        RuleFor(a => a.Contents).NotEmpty().WithErrorCode(ErrorCodes.Required);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateChecklistItemTodo ToTodoCommand(this Request request);
}
