using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Persistence.Abstractions;

public sealed record class PersistenceOptions
{
    public const string Section = "Persistence";

    [Required]
    [MinLength(1)]
    public required string ConnectionString { get; set; }

    [Required]
    [MinLength(1)]
    public required string MigrationsAssembly { get; set; }
}
