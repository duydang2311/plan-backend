using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.TeamIssues.Delete;

namespace WebApp.Api.V1.TeamIssues.Delete;

public sealed record Request
{
    public TeamId? TeamId { get; init; }
    public IssueId? IssueId { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.TeamId).NotNull().WithErrorCode("required");
        RuleFor(a => a.IssueId).NotNull().WithErrorCode("required");
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial DeleteTeamIssue ToCommand(this Request request);
}
