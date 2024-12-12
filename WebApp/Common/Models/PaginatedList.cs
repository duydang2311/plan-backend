namespace WebApp.Common.Models;

public record PaginatedList<T>
{
    public required ICollection<T> Items { get; init; }
    public int TotalCount { get; init; }
}

public static class PaginatedList
{
    public static PaginatedList<T> From<T>(ICollection<T> items, int totalCount)
    {
        return new() { Items = items, TotalCount = totalCount };
    }
}
