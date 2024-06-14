using Microsoft.AspNetCore.WebUtilities;

namespace System;

public static class GuidExtensions
{
    public static string ToBase64(this Guid guid) => Base64UrlTextEncoder.Encode(guid.ToByteArray());
}
