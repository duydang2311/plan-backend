using FastEndpoints;
using FluentValidation;

namespace WebApp.Api.V1.Workspaces.HasWorkspace;

public sealed record class Request
{
    [QueryParam]
    public Guid? Id { get; init; }

    [QueryParam]
    public string? Path { get; init; }
}

public sealed partial class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Id).NotNull().When(x => string.IsNullOrEmpty(x.Path));
        RuleFor(x => x.Path).NotNull().When(x => x.Id is null);
    }
}
