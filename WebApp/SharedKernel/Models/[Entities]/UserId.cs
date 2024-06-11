namespace WebApp.SharedKernel.Models;

public record UserId(string Value)
{
    public static readonly UserId Empty = new(string.Empty);
}
