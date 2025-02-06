using Microsoft.Extensions.Primitives;

namespace FastEndpoints;

public static class FastEndpointsExtensions
{
    public static void ValueParserFor<T>(
        this BindingOptions options,
        Func<StringValues, ParseResult> parse,
        bool handleNull = false
    )
    {
        ParseResult ParseNullable(StringValues values)
        {
            var a = values.FirstOrDefault();
            if (a is null)
            {
                return new ParseResult(true, null);
            }
            return parse(a);
        }

        if (typeof(T).IsValueType)
        {
            options.ValueParserFor<T>(parse);
            if (handleNull)
            {
                options.ValueParserFor(typeof(Nullable<>).MakeGenericType(typeof(T)), ParseNullable);
            }
            return;
        }

        if (handleNull)
        {
            options.ValueParserFor<T>(ParseNullable);
        }
        else
        {
            options.ValueParserFor<T>(parse);
        }
    }
}
