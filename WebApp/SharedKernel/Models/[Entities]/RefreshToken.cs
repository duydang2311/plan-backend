namespace WebApp.SharedKernel.Models;

public readonly record struct RefreshToken(Guid Value)
{
    public static readonly RefreshToken Empty = new(Guid.Empty);
}
