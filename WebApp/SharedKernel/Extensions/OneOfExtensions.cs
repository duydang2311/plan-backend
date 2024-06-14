namespace OneOf;

public static class OneOfExtensions
{
    public static bool TryGetT0<T0>(this OneOf<T0> oneOf, out T0 value)
    {
        if (oneOf.Index == 0)
        {
            value = (T0)oneOf.Value;
            return true;
        }
        value = default!;
        return false;
    }

    public static bool TryGetT0<T0, T1>(this OneOf<T0, T1> oneOf, out T0 value)
    {
        if (oneOf.Index == 0)
        {
            value = (T0)oneOf.Value;
            return true;
        }
        value = default!;
        return false;
    }

    public static bool TryGetT1<T0, T1>(this OneOf<T0, T1> oneOf, out T1 value)
    {
        if (oneOf.Index == 1)
        {
            value = (T1)oneOf.Value;
            return true;
        }
        value = default!;
        return false;
    }
}
