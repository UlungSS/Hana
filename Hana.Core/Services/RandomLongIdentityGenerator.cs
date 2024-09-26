using System.Security.Cryptography;
using Hana.Core.Contracts;

namespace Hana.Core;

public class RandomLongIdentityGenerator : IIdentityGenerator
{
    public string GenerateId()
    {
        var bytes = new byte[8];
        RandomNumberGenerator.Fill(bytes);
        var randomLong = BitConverter.ToInt64(bytes, 0);
        return randomLong.ToString("x");
    }
}