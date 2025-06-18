using System.Text.Json.Serialization;
using Riok.Mapperly.Abstractions;
using WebApp.Api.V1.Common.Dtos;
using WebApp.Common.Models;
using WebApp.Features.Workspaces.SearchItems;

namespace WebApp.Api.V1.Workspaces.SearchItems;

public sealed record Response : PaginatedList<ResponseWorkspaceItem> { }

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(ResponseWorkspaceItemProject), typeDiscriminator: (int)WorkspaceItemKind.Project)]
[JsonDerivedType(typeof(ResponseWorkspaceItemIssue), typeDiscriminator: (int)WorkspaceItemKind.Issue)]
public abstract record ResponseWorkspaceItem
{
    public required double Score { get; init; }
}

public record ResponseWorkspaceItemIssue : ResponseWorkspaceItem
{
    public required BaseIssueDto Item { get; init; }
}

public record ResponseWorkspaceItemProject : ResponseWorkspaceItem
{
    public required BaseProjectDto Item { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[UseStaticMapper(typeof(DtoMapper))]
public static partial class ResponseMapper
{
    public static partial ResponseWorkspaceItemProject ToResponse(this WorkspaceItemProject item);

    public static partial ResponseWorkspaceItemIssue ToResponse(this WorkspaceItemIssue item);

    public static Response ToResponse(this PaginatedList<WorkspaceItem> result)
    {
        return new Response
        {
            Items =
            [
                .. result.Items.Select<WorkspaceItem, ResponseWorkspaceItem>(item =>
                    item switch
                    {
                        WorkspaceItemProject project => project.ToResponse(),
                        WorkspaceItemIssue issue => issue.ToResponse(),
                        _ => throw new NotSupportedException($"Unsupported workspace item type: {item.GetType().Name}"),
                    }
                ),
            ],
            TotalCount = result.TotalCount,
        };
    }
}
