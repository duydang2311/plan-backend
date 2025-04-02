using System.Linq.Expressions;
using WebApp.Common.Constants;
using WebApp.Common.Helpers;

namespace WebApp.Common.Models;

public class Orderable
{
    private string UppercasedName =>
        string.Join(
            '.',
            Name.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(a => string.Concat(a[0].ToString().ToUpperInvariant(), a.AsSpan(1)))
        );

    public required string Name { get; set; }
    public Order Order { get; set; } = Order.Ascending;

    public IOrderedQueryable<T> Sort<T>(IQueryable<T> query)
        where T : notnull
    {
        if (query.Expression.Type == typeof(IOrderedQueryable<T>))
        {
            return Sort((IOrderedQueryable<T>)query);
        }
        return Order == Order.Ascending
            ? query.OrderBy(ExpressionHelper.OrderBy<T>(UppercasedName))
            : query.OrderByDescending(ExpressionHelper.OrderBy<T>(UppercasedName));
    }

    public IOrderedQueryable<T> Sort<T>(IOrderedQueryable<T> query)
        where T : notnull
    {
        return Order == Order.Ascending
            ? query.ThenBy(ExpressionHelper.OrderBy<T>(UppercasedName))
            : query.ThenByDescending(ExpressionHelper.OrderBy<T>(UppercasedName));
    }

    public IOrderedQueryable<T1> Sort<T1, T2>(IQueryable<T1> query, Expression<Func<T1, T2>> expression)
        where T1 : notnull
    {
        return Order == Order.Ascending ? query.OrderBy(expression) : query.OrderByDescending(expression);
    }
}
