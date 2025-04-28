using System.ComponentModel.DataAnnotations;

namespace WebApp.Api.V1.Common;

public sealed record IdEncodingOptions
{
    public const string Section = "IdEncoding";

    [Required]
    public required string Alphabet { get; init; }
}
