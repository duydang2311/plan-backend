using Microsoft.Extensions.Options;
using Sqids;

namespace WebApp.Common.IdEncoding;

public sealed class SqidsEncoder(IOptions<IdEncoderOptions> options) : IIdEncoder
{
    SqidsEncoder<long>? encoder;

    public string Encode(long id)
    {
        return GetOrCreateLongEncoder().Encode(id);
    }

    public bool TryDecode(string encoded, out long id)
    {
        var list = GetOrCreateLongEncoder().Decode(encoded);
        if (list.Count > 0)
        {
            id = list[0];
            return true;
        }
        id = default;
        return false;
    }

    public bool TryDecode(ReadOnlySpan<char> encoded, out long id)
    {
        var list = GetOrCreateLongEncoder().Decode(encoded);
        if (list.Count > 0)
        {
            id = list[0];
            return true;
        }
        id = default;
        return false;
    }

    SqidsEncoder<long> GetOrCreateLongEncoder()
    {
        return encoder ??= new SqidsEncoder<long>(
            new SqidsOptions { Alphabet = options.Value.Alphabet, MinLength = 8 }
        );
    }
}
