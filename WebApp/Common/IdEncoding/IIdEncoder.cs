namespace WebApp.Common.IdEncoding;

public interface IIdEncoder
{
    string Encode(long id);
    bool TryDecode(string encoded, out long id);
    bool TryDecode(ReadOnlySpan<char> encoded, out long id);
}
