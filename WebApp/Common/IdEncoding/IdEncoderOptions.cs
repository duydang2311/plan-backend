using System.ComponentModel.DataAnnotations;

namespace WebApp.Common.IdEncoding;

public sealed record IdEncoderOptions
{
    public const string Section = "IdEncoder";

    [Required]
    public required string Alphabet { get; init; }
}
