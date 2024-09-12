using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace WebApp.Common.Helpers;

public static class ExpressionHelper
{
    public static Expression<Func<T, T>> LambdaNew<T>(string names)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        return Expression.Lambda<Func<T, T>>(Init(typeof(T), parameter, names), parameter);
    }

    public static Expression LambdaProperty<T>(string name)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, typeof(T).GetProperty(name)!);
        return Expression.Lambda(property, parameter);
    }

    public static Expression<Func<T, T>> Append<T>(Expression<Func<T, T>>? left, Expression<Func<T, T>> right)
    {
        if (left is null)
        {
            return right;
        }

        var replace = new ReplacingExpressionVisitor(right.Parameters, [left.Body]);
        var combined = replace.Visit(right.Body);
        return Expression.Lambda<Func<T, T>>(combined, left.Parameters);
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
                        string.Join(
                            ',',
                            splits.Where(x => x.StartsWith(format)).Select(x => x.Substring(format.Length))
                        )
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

    // ref: https://stackoverflow.com/a/66334073
    public static Expression<Func<TSource, TTarget>> Select<TSource, TTarget>(string members) =>
        Select<TSource, TTarget>(members.Split(',').Select(m => m.Trim()));

    public static Expression<Func<TSource, TTarget>> Select<TSource, TTarget>(IEnumerable<string> members)
    {
        var parameter = Expression.Parameter(typeof(TSource), "e");
        var body = NewObject(typeof(TTarget), parameter, members.Select(m => m.Split('.')));
        return Expression.Lambda<Func<TSource, TTarget>>(body, parameter);
    }

    private static MemberInitExpression NewObject(
        Type targetType,
        Expression source,
        IEnumerable<string[]> memberPaths,
        int depth = 0
    )
    {
        var bindings = new List<MemberBinding>();
        var target = Expression.Constant(null, targetType);
        foreach (var memberGroup in memberPaths.GroupBy(path => path[depth]))
        {
            var memberName = memberGroup.Key;
            var targetMember = Expression.PropertyOrField(target, memberName);
            var sourceMember = Expression.PropertyOrField(source, memberName);
            var childMembers = memberGroup.Where(path => depth + 1 < path.Length).ToList();

            Expression? targetValue = null;
            if (childMembers.Count == 0)
            {
                targetValue = sourceMember;
            }
            else
            {
                if (
                    IsEnumerableType(targetMember.Type, out var sourceElementType)
                    && IsEnumerableType(targetMember.Type, out var targetElementType)
                )
                {
                    var sourceElementParam = Expression.Parameter(sourceElementType, "e");
                    targetValue = NewObject(targetElementType, sourceElementParam, childMembers, depth + 1);
                    targetValue = Expression.Call(
                        typeof(Enumerable),
                        nameof(Enumerable.Select),
                        [sourceElementType, targetElementType],
                        sourceMember,
                        Expression.Lambda(targetValue, sourceElementParam)
                    );

                    targetValue = CorrectEnumerableResult(targetValue, targetElementType, targetMember.Type);
                }
                else
                {
                    targetValue = NewObject(targetMember.Type, sourceMember, childMembers, depth + 1);
                }
            }

            bindings.Add(Expression.Bind(targetMember.Member, targetValue));
        }
        return Expression.MemberInit(Expression.New(targetType), bindings);
    }

    static bool IsEnumerableType(Type type, out Type elementType)
    {
        foreach (var intf in type.GetInterfaces())
        {
            if (intf.IsGenericType && intf.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                elementType = intf.GetGenericArguments()[0];
                return true;
            }
        }

        elementType = null!;
        return false;
    }

    static bool IsSameCollectionType(Type type, Type genericType, Type elementType)
    {
        var result = genericType.MakeGenericType(elementType).IsAssignableFrom(type);
        return result;
    }

    static Expression CorrectEnumerableResult(Expression enumerable, Type elementType, Type memberType)
    {
        if (memberType == enumerable.Type)
        {
            return enumerable;
        }

        if (memberType.IsArray)
        {
            return Expression.Call(typeof(Enumerable), nameof(Enumerable.ToArray), [elementType], enumerable);
        }

        if (
            IsSameCollectionType(memberType, typeof(List<>), elementType)
            || IsSameCollectionType(memberType, typeof(ICollection<>), elementType)
            || IsSameCollectionType(memberType, typeof(IReadOnlyList<>), elementType)
            || IsSameCollectionType(memberType, typeof(IReadOnlyCollection<>), elementType)
        )
        {
            return Expression.Call(typeof(Enumerable), nameof(Enumerable.ToList), [elementType], enumerable);
        }

        throw new NotImplementedException($"Not implemented transformation for type '{memberType.Name}'");
    }
}
