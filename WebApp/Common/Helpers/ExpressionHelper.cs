using System.Linq.Expressions;

namespace WebApp.Common.Helpers;

public static class ExpressionHelper
{
    public static Expression<Func<T, T>> New<T>(string names)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        return Expression.Lambda<Func<T, T>>(Init(typeof(T), parameter, names), parameter);
    }

    private static MemberInitExpression Init(Type type, Expression member, string names)
    {
        var processed = new HashSet<string>();
        var splits = names.Split(',').Select(x => x.Trim()).ToArray();
        var bindings = new List<MemberBinding>();
        for (var i = 0; i != splits.Length; ++i)
        {
            var name = splits[i];
            switch (name.Split('.'))
            {
                case [string Name]:
                    {
                        var property = type.GetProperty(Name)!;
                        bindings.Add(Expression.Bind(property, Expression.Property(member, property)));
                        break;
                    }
                case [string Name, _]:
                    {
                        if (processed.Contains(Name))
                        {
                            break;
                        }
                        processed.Add(Name);
                        var property = type.GetProperty(Name)!;
                        var format = $"{Name}.";
                        var init = Init(
                            property.PropertyType,
                            Expression.Property(member, Name),
                            string.Join(',', splits
                                .Where(x => x.StartsWith(format))
                                .Select(x => x.Substring(format.Length)))
                        );

                        bindings.Add(Expression.Bind(property, init));
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }
        return Expression.MemberInit(Expression.New(type), bindings);
    }
}
