using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Users.GetMany;

namespace WebApp.Api.V1.Users.GetMany;

public sealed record Request : Collective
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Select { get; init; }
}

public sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(a => a.WorkspaceId).NotNull().WithErrorCode("required");
    }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetUsers ToCommand(this Request request);
}
