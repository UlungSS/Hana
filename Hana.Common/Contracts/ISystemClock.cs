namespace Hana.Common.Contracts;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}