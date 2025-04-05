using FastEndpoints;

namespace WebApp.Common.Models;

public record KeysetPagination<T> : IKeysetPagination<T>
{
    private int size = 20;

    [QueryParam]
    public T? Cursor { get; init; }

    [QueryParam]
    public int Size
    {
        get => size;
        init { size = value < 0 ? 0 : value; }
    }

    [BindFrom("order")]
    public Orderable[] Order { get; init; } = [];
}
