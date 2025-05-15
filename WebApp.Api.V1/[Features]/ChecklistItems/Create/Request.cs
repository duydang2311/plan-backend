using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.ChecklistItems.Create;

namespace WebApp.Api.V1.ChecklistItems.Create;

public sealed record Request
{
    public IssueId? ParentIssueId { get; init; }
    public ChecklistItemKind? Kind { get; init; }
    public string? Content { get; init; }
    public IssueId? SubIssueId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.ParentIssueId).NotNull().WithErrorCode(ErrorCodes.Required);

        RuleFor(x => x.Kind)
            .NotNull()
            .WithErrorCode(ErrorCodes.Required)
            .IsInEnum()
            .WithErrorCode(ErrorCodes.InvalidValue);

        When(
            a => a.Kind.HasValue && a.Kind.Value == ChecklistItemKind.Todo,
            () =>
            {
                RuleFor(x => x.Content).NotEmpty().WithErrorCode(ErrorCodes.Required);
            }
        );

        When(
            a => a.Kind.HasValue && a.Kind.Value == ChecklistItemKind.SubIssue,
            () =>
            {
                RuleFor(x => x.SubIssueId).NotNull().WithErrorCode(ErrorCodes.Required);
            }
        );
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial CreateChecklistItemTodo ToTodoCommand(this Request request);

    public static partial CreateChecklistItemSubIssue ToSubIssueCommand(this Request request);
}
