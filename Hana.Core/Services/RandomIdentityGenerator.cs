using Hana.Core.Contracts;
using System.Security.Cryptography;

namespace Hana.Core;

public class RandomIdentityGenerator(int size) : IIdentityGenerator
{
    private readonly int _size = size;
    public string GenerateId()
    {
        if (_size == 4 || _size == 8)
        {
            var bytes = new byte[_size];
            RandomNumberGenerator.Fill(bytes);
            var randomShort = _size == 4 ? BitConverter.ToInt32(bytes, 0) : BitConverter.ToInt64(bytes, 0);
            return randomShort.ToString("x");
        }
        return null!;
    }
}
