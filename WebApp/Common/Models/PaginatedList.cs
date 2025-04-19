namespace WebApp.Common.Models;

public record PaginatedList<T>
{
    public required IReadOnlyCollection<T> Items { get; init; }
    public int TotalCount { get; init; }
}

public static class PaginatedList
{
    public static PaginatedList<T> From<T>(IReadOnlyCollection<T> items, int totalCount)
    {
        return new() { Items = items, TotalCount = totalCount };
    }
}
