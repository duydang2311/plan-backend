namespace System;

public static class StringExtensions
{
    public static bool EqualsEither(
        this string self,
        string[] inputs,
        StringComparison comparison = StringComparison.CurrentCulture
    )
    {
        return Array.Exists(inputs, input => self.Equals(input, comparison));
    }
}
