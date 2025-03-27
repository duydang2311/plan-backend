using FastEndpoints;

namespace WebApp.Common.Models;

public record KeysetPagination<T>
{
    private int size = 20;
    private T? cursor;

    [QueryParam]
    public T? Cursor
    {
        get => cursor;
        set { cursor = value; }
    }

    [QueryParam]
    public int Size
    {
        get => size;
        set { size = value < 0 ? 0 : value; }
    }

    [BindFrom("order")]
    public Orderable[] Order { get; set; } = [];
}
