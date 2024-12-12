using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;

namespace WebApp.Common.Helpers;

public static class ExpressionHelper
{
    private static readonly NullabilityInfoContext nullabilityInfoContext = new();

    public static Expression<Func<T, object>> OrderBy<T>(string name)
    {
        var parameter = Expression.Parameter(typeof(T), "a");
        return Expression.Lambda<Func<T, object>>(
            Expression.TypeAs(BuildOrderBy(typeof(T), parameter, name), typeof(object)),
            parameter
        );
    }

    public static Expression<Func<T, T>> LambdaNew<T>(string names)
    {
        var parameter = Expression.Parameter(typeof(T), "a");
        return Expression.Lambda<Func<T, T>>(Init(typeof(T), parameter, names), parameter);
    }

    public static Expression LambdaProperty<T>(string name)
    {
        var parameter = Expression.Parameter(typeof(T), "a");
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
        var splits = names.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToArray();
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

    private static MemberExpression BuildOrderBy(Type type, Expression member, string name)
    {
        switch (name.Split('.'))
        {
            case [string Name]:
            {
                var property = type.GetProperty(Name)!;
                return Expression.Property(member, property);
            }
            case [string Name, string Rest]:
            {
                var property = type.GetProperty(Name)!;
                return BuildOrderBy(property.PropertyType, Expression.Property(member, property), Rest);
            }
            default:
                throw new NotImplementedException();
        }
    }

    // ref: https://stackoverflow.com/a/66334073
    public static Expression<Func<TSource, TTarget>> Select<TSource, TTarget>(string members) =>
        Select<TSource, TTarget>(
            members.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToArray()
        );

    public static Expression<Func<TSource, TTarget>> Select<TSource, TTarget>(IEnumerable<string> members)
    {
        var parameter = Expression.Parameter(typeof(TSource), "e");
        var body = NewObject(typeof(TTarget), parameter, members.Select(m => m.Split('.')));
        return Expression.Lambda<Func<TSource, TTarget>>(body, parameter);
    }

    public static MemberInitExpression Init<TSource, TTarget>(ParameterExpression parameter, string members)
    {
        return NewObject(
            typeof(TTarget),
            parameter,
            members
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Split('.'))
        );
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
            var targetMember = Expression.Property(target, memberName);
            var sourceMember = Expression.Property(source, memberName);
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

            if (
                (sourceMember.Type.IsValueType && Nullable.GetUnderlyingType(sourceMember.Type) is not null)
                || IsNullableMember(sourceMember.Member)
            )
            {
                targetValue = Expression.Condition(
                    Expression.Equal(sourceMember, Expression.Constant(null)),
                    Expression.Constant(null, targetMember.Type),
                    targetValue
                );
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

    static bool IsNullableMember(MemberInfo member)
    {
        return member switch
        {
            PropertyInfo m
                => nullabilityInfoContext.Create(m)
                    is { WriteState: NullabilityState.Nullable }
                        or { ReadState: NullabilityState.Nullable },
            FieldInfo m
                => nullabilityInfoContext.Create(m)
                    is { WriteState: NullabilityState.Nullable }
                        or { ReadState: NullabilityState.Nullable },
            EventInfo m
                => nullabilityInfoContext.Create(m)
                    is { WriteState: NullabilityState.Nullable }
                        or { ReadState: NullabilityState.Nullable },
            _ => false,
        };
    }
}
