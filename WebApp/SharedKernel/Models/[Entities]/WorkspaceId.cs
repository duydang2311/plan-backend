namespace WebApp.SharedKernel.Models;

public readonly record struct WorkspaceId(Guid Value)
{
    public static readonly WorkspaceId Empty = new(Guid.Empty);
}
