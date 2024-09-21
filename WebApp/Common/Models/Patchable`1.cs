using System.Linq.Expressions;

namespace WebApp.Common.Models;

public abstract record Patchable<T> : Patchable
    where T : Patchable<T>
{
    public bool Has<TProperty>(Expression<Func<T, TProperty>> expression) =>
        PresentProperties.Contains(((MemberExpression)expression.Body).Member.Name);

    public bool TryGetValue<TProperty>(Expression<Func<T, TProperty>> expression, out TProperty value)
    {
        if (!Has(expression))
        {
            value = default!;
            return false;
        }
        value = (TProperty)GetType().GetProperty(((MemberExpression)expression.Body).Member.Name)!.GetValue(this)!;
        return true;
    }
}
