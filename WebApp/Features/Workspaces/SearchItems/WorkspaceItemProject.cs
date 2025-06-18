using WebApp.Domain.Entities;

namespace WebApp.Features.Workspaces.SearchItems;

public record WorkspaceItemProject : WorkspaceItem
{
    public required Project Item { get; init; }
}
