namespace WebApp.SharedKernel.Hashers.Abstractions;

public interface IHasher
{
    byte[] Hash(string input, byte[] salt);
    bool Verify(string input, byte[] hash, byte[] salt);
}
