using System.ComponentModel.DataAnnotations;

namespace WebApp.Common.Models;

public class FastEndpointsOptions
{
    public const string Section = "FastEndpoints";

    [Required]
    public required string[] Assemblies { get; set; }
}
