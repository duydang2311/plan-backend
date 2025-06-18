using WebApp.Domain.Entities;

namespace WebApp.Features.Workspaces.SearchItems;

public record WorkspaceItemIssue : WorkspaceItem
{
    public required Issue Item { get; init; }
}
