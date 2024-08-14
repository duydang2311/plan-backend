using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.IssueComments.Create;

namespace WebApp.Api.V1.IssueComments.Create;

public sealed record Request
{
    public IssueId IssueId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId AuthorId { get; init; }

    public string? Content { get; init; }

}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial CreateIssueComment ToCommand(this Request request);
}
