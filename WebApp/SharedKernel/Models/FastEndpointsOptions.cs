using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Models;

public class FastEndpointsOptions
{
    public const string Section = "FastEndpoints";

    [Required]
    public required string[] Assemblies { get; set; }
}
