using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Models;

public sealed record class JwtOptions
{
    public const string Section = "Jwt";

    [Required] public required string[] ValidIssuers { get; set; }
    [Required] public required string[] ValidAudiences { get; set; }
    [Required] public required string CertificateFilePath { get; set; }
    [Required] public required string KeyFilePath { get; set; }
    [Required] public required string KeyPassword { get; set; }
}
