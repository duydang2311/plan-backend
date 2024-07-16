namespace WebApp.Domain.Entities;

public readonly record struct UserId(Guid Value)
{
    public static readonly UserId Empty = new(Guid.Empty);

    public override string ToString()
    {
        return Value.ToString();
    }
}
