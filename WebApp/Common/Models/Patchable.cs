namespace WebApp.Common.Models;

public abstract record Patchable
{
    public HashSet<string> PresentProperties { get; init; } = [];
}
