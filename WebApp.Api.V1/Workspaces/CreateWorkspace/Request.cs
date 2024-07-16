using System.Security.Claims;
using System.Text.RegularExpressions;
using FastEndpoints;
using FluentValidation;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Workspaces.CreateWorkspace;

public sealed record class Request(string? Name, string? Path)
{
    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

public sealed partial class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Path).NotEmpty().Must(x => !InvalidNamePattern().IsMatch(x!)).WithErrorCode("pattern");
    }

    [GeneratedRegex("[^a-zA-Z0-9\\-_]", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex InvalidNamePattern();
}
