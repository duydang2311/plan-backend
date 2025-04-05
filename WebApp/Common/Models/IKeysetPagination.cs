namespace WebApp.Common.Models;

public interface IKeysetPagination<T>
{
    T? Cursor { get; }
    int Size { get; }
    Orderable[] Order { get; }
}
