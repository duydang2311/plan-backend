using Norgerman.Cryptography.Scrypt;
using WebApp.SharedKernel.Hashers.Abstractions;

namespace WebApp.SharedKernel.Hashers;

public sealed class ScryptHasher : IHasher
{
    public byte[] Hash(string input, byte[] salt)
    {
        return ScryptUtil.Scrypt(input, salt, N: 131072, r: 8, p: 1, dkLen: 64);
    }

    public bool Verify(string input, byte[] hash, byte[] salt)
    {
        return ScryptUtil.Scrypt(input, salt, N: 131072, r: 8, p: 1, dkLen: 64).SequenceEqual(hash);
    }
}
