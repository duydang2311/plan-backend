using System.ComponentModel.DataAnnotations;

namespace WebApp.Hubs.Common;

public sealed record ApiOptions
{
    public const string Section = "Api";

    [Required]
    public required string HttpClientName { get; init; }

    [Required]
    public required string BaseUrl { get; init; }

    [Required]
    public required string AccessToken { get; init; }
}
