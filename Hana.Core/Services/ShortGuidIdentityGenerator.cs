using DEDrake;
using Hana.Core.Contracts;

namespace Hana.Core;

public class ShortGuidIdentityGenerator : IIdentityGenerator
{
    public string GenerateId() => ShortGuid.NewGuid().ToString();
}