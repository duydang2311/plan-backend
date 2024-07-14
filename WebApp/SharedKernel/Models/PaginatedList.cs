namespace WebApp.SharedKernel.Models;

public record PaginatedList<T>
{
    public required ICollection<T> Items { get; init; }
    public int TotalCount { get; init; }
}
