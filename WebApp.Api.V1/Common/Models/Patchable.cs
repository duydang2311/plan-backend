namespace WebApp.Api.V1.Common.Models;

public abstract record Patchable
{
    private readonly HashSet<string> presentProperties = [];

    public void MarkPropertyAsPresent(string propertyName) => presentProperties.Add(propertyName);

    public bool IsPresent(string propertyName) => presentProperties.Contains(propertyName);
}
