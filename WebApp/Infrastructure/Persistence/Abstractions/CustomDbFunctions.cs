using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApp.Infrastructure.Persistence.Abstractions;

public static class CustomDbFunctions
{
    public static string ImmutableUnaccent(string text) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(ImmutableUnaccent)));
}
