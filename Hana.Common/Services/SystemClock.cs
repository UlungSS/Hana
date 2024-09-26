using Hana.Common.Contracts;

namespace Hana.Common.Services;

public class SystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}