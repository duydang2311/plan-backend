using System.Text.Json;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;

namespace WebApp.Features.IssueAudits.Patch;

public sealed record PatchIssueAudit : ICommand<OneOf<NotFoundError, Success>>
{
    public required long Id { get; init; }
    public required Patchable Patch { get; init; }

    public sealed record Patchable : Patchable<Patchable>
    {
        public JsonDocument? Data { get; init; }
    }
}
