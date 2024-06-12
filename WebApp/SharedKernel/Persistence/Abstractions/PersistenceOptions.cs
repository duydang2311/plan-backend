using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Persistence.Abstractions;

public sealed record class PersistenceOptions
{
    public const string Section = "Persistence";

    [Required]
    public required string ConnectionString { get; init; }

    [Required]
    public required string MigrationsAssembly { get; init; }
}
