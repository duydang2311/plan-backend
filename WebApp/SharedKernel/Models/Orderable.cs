using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Constants;

namespace WebApp.SharedKernel.Models;

public class Orderable
{
    private string UppercasedName => string.Concat(Name[0].ToString().ToUpperInvariant(), Name.AsSpan(1));

    public required string Name { get; set; }
    public Order Order { get; set; } = Order.Ascending;

    public IOrderedQueryable<T> Sort<T>(IQueryable<T> query)
        where T : notnull
    {
        return Order == Order.Ascending
            ? query.OrderBy(x => EF.Property<T>(x, UppercasedName))
            : query.OrderByDescending(x => EF.Property<T>(x, UppercasedName));
    }

    public IOrderedQueryable<T> Sort<T>(IOrderedQueryable<T> query)
        where T : notnull
    {
        return Order == Order.Ascending
            ? query.ThenBy(x => EF.Property<T>(x, UppercasedName))
            : query.ThenByDescending(x => EF.Property<T>(x, UppercasedName));
    }

    public IOrderedQueryable<T1> Sort<T1, T2>(IQueryable<T1> query, Expression<Func<T1, T2>> expression)
        where T1 : notnull
    {
        return Order == Order.Ascending ? query.OrderBy(expression) : query.OrderByDescending(expression);
    }
}
