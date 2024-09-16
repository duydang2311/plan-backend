using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Projects.GetOne;

public sealed record GetProject : ICommand<OneOf<ValidationFailures, None, Project>>
{
    public ProjectId? ProjectId { get; init; }
    public string? Identifier { get; init; }
    public string? Select { get; init; }
}
