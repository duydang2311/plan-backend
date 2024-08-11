using FastEndpoints;

namespace WebApp.Common.Models;

public record Collective
{
    private int page = 1;
    private int size = 20;
    private int? offset;

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

    [QueryParam]
    public int Offset
    {
        get => offset.GetValueOrDefault((page - 1) * Size);
        set { offset = value < 0 ? 0 : value; }
    }

    [BindFrom("order")]
    public Orderable[] Order { get; set; } = [];
}
