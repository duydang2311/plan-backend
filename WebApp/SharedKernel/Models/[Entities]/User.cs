using NanoidDotNet;

namespace WebApp.SharedKernel.Models;

public sealed record class User
{
    public UserId Id { get; init; } = UserId.Empty;
}
