namespace WebApp.Common.Models;

public static class OrderableExtensions
{
    public static IQueryable<T> SortOrDefault<T>(
        this IEnumerable<Orderable> orders,
        IQueryable<T> query,
        Func<IQueryable<T>, IOrderedQueryable<T>> sortDefault
    )
        where T : notnull
    {
        if (!orders.Any())
        {
            return sortDefault(query);
        }

        var orderedQuery = orders.First().Sort(query);
        foreach (var order in orders.Skip(1))
        {
            orderedQuery = order.Sort(orderedQuery);
        }
        return orderedQuery;
    }
}
