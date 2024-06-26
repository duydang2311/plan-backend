namespace WebApp.SharedKernel.Models;

public readonly record struct UserId(Guid Value)
{
    public static readonly UserId Empty = new(Guid.Empty);
}
