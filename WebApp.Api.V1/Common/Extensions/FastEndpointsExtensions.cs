namespace FastEndpoints;

public static class FastEndpointsExtensions
{
    public static bool ValueParserFor<T>(
        this BindingOptions options,
        Func<object?, ParseResult> parse,
        bool handleNull = false
    )
    {
        var ok = options.ValueParserFor<T>(parse);
        if (handleNull)
        {
            ok = options.ValueParserFor<T?>(
                (input) =>
                {
                    if (input is null)
                    {
                        return new ParseResult(true, null);
                    }
                    return parse(input);
                }
            );
        }
        return ok;
    }
}
