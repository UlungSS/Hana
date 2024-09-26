using System.Security.Cryptography;
using System.Text;

using Hana.Identity.Contracts;
using Hana.Identity.Models;

namespace Hana.Identity.Services;


public class DefaultSecretHasher : ISecretHasher
{
    public HashedSecret HashSecret(string secret)
    {
        var saltBytes = GenerateSalt();
        return HashSecret(secret, saltBytes);
    }

    public HashedSecret HashSecret(string secret, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(secret);
        var hashedPassword = HashSecret(passwordBytes, salt);
        return HashedSecret.FromBytes(hashedPassword, salt);
    }

    public bool VerifySecret(string clearTextSecret, string secret, string salt)
    {
        var hashedPassword = HashedSecret.FromString(secret, salt);
        return VerifySecret(clearTextSecret, hashedPassword);
    }

    public bool VerifySecret(string clearTextSecret, HashedSecret hashedSecret)
    {
        var password = hashedSecret.Secret;
        var salt = hashedSecret.Salt;
        var providedHashedPassword = HashSecret(clearTextSecret, salt);
        return providedHashedPassword.Secret.SequenceEqual(password);
    }

    public byte[] HashSecret(byte[] secret, byte[] salt)
    {
        var passwordAndSalt = secret.Concat(salt).ToArray();
        return SHA256.HashData(passwordAndSalt);
    }

    public byte[] GenerateSalt(int saltSize = 32) => RandomNumberGenerator.GetBytes(saltSize);
}