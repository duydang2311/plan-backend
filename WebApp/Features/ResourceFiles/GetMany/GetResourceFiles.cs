using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ResourceFiles.GetMany;

public sealed record GetResourceFiles : KeysetPagination<ResourceFileId?>, ICommand<PaginatedList<ResourceFile>>
{
    public required ResourceId ResourceId { get; init; }
    public string? Select { get; init; }
}
