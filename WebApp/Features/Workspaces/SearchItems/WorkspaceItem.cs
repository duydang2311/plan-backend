using System.Text.Json.Serialization;

namespace WebApp.Features.Workspaces.SearchItems;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(WorkspaceItemProject), typeDiscriminator: (int)WorkspaceItemKind.Project)]
[JsonDerivedType(typeof(WorkspaceItemIssue), typeDiscriminator: (int)WorkspaceItemKind.Issue)]
public abstract record WorkspaceItem
{
    public required double Score { get; init; }
}
