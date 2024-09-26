using Hana.Core.Contracts;

namespace Hana.Core;

public class GuidIdentityGenerator : IIdentityGenerator
{
    public string GenerateId() => Guid.NewGuid().ToString("N");
}