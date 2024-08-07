using System.Text.Json.Serialization;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using Riok.Mapperly.Abstractions;

namespace WebApp.Common.Models;

public record Collective
{
    private int page = 1;
    private int size = 20;

    [QueryParam]
    public int Page
    {
        get => page;
        set { page = value < 1 ? 1 : value; }
    }

    [QueryParam]
    public int Size
    {
        get => size;
        set { size = value < 0 ? 0 : value; }
    }

    [JsonIgnore]
    [MapperIgnore]
    public int Offset => (page - 1) * Size;

    [FromQuery]
    [BindFrom("order")]
    public Orderable[] Order { get; set; } = [];
}
