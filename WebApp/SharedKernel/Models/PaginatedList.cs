namespace WebApp.SharedKernel.Models;

public record PaginatedList<T>
{
    public required ICollection<T> Items { get; set; }
    public int TotalCount { get; set; }
}
