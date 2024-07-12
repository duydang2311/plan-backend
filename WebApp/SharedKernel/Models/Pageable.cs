namespace WebApp.SharedKernel.Models;

public sealed record Pageable
{
    private int page = 1;
    private int size = 20;

    public int Page
    {
        get => page;
        set { page = value < 1 ? 1 : value; }
    }

    public int Size
    {
        get => size;
        set { size = value < 0 ? 0 : value; }
    }

    public int Offset => (page - 1) * Size;

    public Pageable() { }

    public Pageable(int page, int size)
    {
        Page = page;
        Size = size;
    }
}
