using System.Security.Cryptography;
using Hana.Identity.Models;

namespace Hana.Identity.Contracts;

public interface ISecretHasher
{
    HashedSecret HashSecret(string secret);
    HashedSecret HashSecret(string secret, byte[] salt);
    byte[] HashSecret(byte[] secret, byte[] salt);

    bool VerifySecret(string clearTextSecret, string secret, string salt);

    bool VerifySecret(string clearTextSecret, HashedSecret hashedSecret);

    byte[] GenerateSalt(int saltSize = 32) => RandomNumberGenerator.GetBytes(saltSize);
}