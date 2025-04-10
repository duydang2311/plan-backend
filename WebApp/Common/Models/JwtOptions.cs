using System.ComponentModel.DataAnnotations;

namespace WebApp.Common.Models;

public sealed record class JwtOptions
{
    public const string Section = "Jwt";

    [Required]
    public required string Issuer { get; set; }

    [Required]
    public required string PrivateKey { get; set; }

    [Required]
    public required string PublicKey { get; set; }
}
