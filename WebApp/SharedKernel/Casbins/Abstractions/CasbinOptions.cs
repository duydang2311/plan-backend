using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Casbins.Abstractions;

public sealed record class CasbinOptions
{
    public const string Section = "Casbin";

    [Required]
    public required string ConnectionString { get; init; }

    [Required]
    public required string ModelPath { get; init; }
}
