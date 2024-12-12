using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.IssueAssignees.Create;

namespace WebApp.Api.V1.IssueAssignees.Create;

public sealed record Request
{
    public IssueId? IssueId { get; init; }
    public UserId? UserId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.IssueId).NotNull().WithErrorCode("required");
        RuleFor(a => a.UserId).NotNull().WithErrorCode("required");
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial CreateIssueAssignee ToCommand(this Request request);
}
