using System.Linq.Expressions;

namespace WebApp.Api.V1.Common.Models;

public abstract record Patchable<T> : Patchable
    where T : Patchable<T>
{
    public bool IsPresent<TProperty>(Expression<Func<T, TProperty>> expression) =>
        IsPresent(((MemberExpression)expression.Body).Member.Name);

    public bool TryGetValue<TProperty>(Expression<Func<T, TProperty>> expression, out TProperty value)
    {
        if (!IsPresent(expression))
        {
            value = default!;
            return false;
        }
        value = (TProperty)GetType().GetProperty(((MemberExpression)expression.Body).Member.Name)!.GetValue(this)!;
        return true;
    }
}
