using EntityFrameworkCore.Projectables;
using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record Resource
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public ResourceId Id { get; init; }
    public string Name { get; init; } = null!;
    public UserId CreatorId { get; init; }
    public User Creator { get; init; } = null!;
    public string Rank { get; init; } = null!;
    public ResourceDocument? Document { get; init; }
    public ICollection<ResourceFile> Files { get; init; } = null!;
    public WorkspaceResource? WorkspaceResource { get; init; }
    public ProjectResource? ProjectResource { get; init; }

    [Projectable(UseMemberBody = nameof(previewFileCount))]
    public int PreviewFileCount { get; init; }

    [Projectable(UseMemberBody = nameof(previewFileMimeTypes))]
    public IReadOnlyCollection<string> PreviewFileMimeTypes { get; init; } = null!;

    private int previewFileCount => Files.Count;
    private IReadOnlyCollection<string> previewFileMimeTypes =>
        Files.OrderByDescending(a => a.Id).Select(a => a.MimeType).Distinct().Take(5).ToList();
}
