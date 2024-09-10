namespace WebApp.Domain.Entities;

public readonly record struct ProjectId : IEntityGuid
{
    public Guid Value { get; init; }

    public static readonly ProjectId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }

    public static bool TryParse(string? input, out ProjectId? output) //adhere to this signature
    {
        output = null;

        if (Guid.TryParseExact(input, "D", out var guid))
        {
            return false;
        }

        output = new ProjectId { Value = guid };
        return true;
    }
}
