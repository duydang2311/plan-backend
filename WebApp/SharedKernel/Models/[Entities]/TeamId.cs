namespace WebApp.SharedKernel.Models;

public readonly record struct TeamId(Guid Value)
{
    public static readonly TeamId Empty = new(Guid.Empty);

    public override string ToString()
    {
        return Value.ToString();
    }
}
