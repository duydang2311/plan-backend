namespace FastEndpoints;

public static class FastEndpointsExtensions
{
    public static bool ValueParserFor<T>(
        this BindingOptions options,
        Func<object?, ParseResult> parse,
        bool handleNull = false
    )
    {
        ParseResult ParseNullable(object? input)
        {
            if (input is null)
            {
                return new ParseResult(true, null);
            }
            return parse(input);
        }

        if (typeof(T).IsValueType)
        {
            var ok = options.ValueParserFor<T>(parse);
            if (handleNull)
            {
                ok = options.ValueParserFor(typeof(Nullable<>).MakeGenericType(typeof(T)), ParseNullable);
            }
            return ok;
        }

        return handleNull ? options.ValueParserFor<T>(ParseNullable) : options.ValueParserFor<T>(parse);
    }
}
