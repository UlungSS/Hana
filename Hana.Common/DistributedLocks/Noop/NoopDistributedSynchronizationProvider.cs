using Medallion.Threading;

namespace Hana.Common.DistributedLocks.Noop;

public class NoopDistributedSynchronizationProvider : IDistributedLockProvider
{
    public IDistributedLock CreateLock(string name)
    {
        return new NoopDistributedLock();
    }
}