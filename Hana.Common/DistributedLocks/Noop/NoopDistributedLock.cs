using Medallion.Threading;

namespace Hana.Common.DistributedLocks.Noop;

public class NoopDistributedLock : IDistributedLock
{
    public IDistributedSynchronizationHandle? TryAcquire(TimeSpan timeout = new TimeSpan(), CancellationToken cancellationToken = new CancellationToken())
    {
        return new NoopDistributedSynchronizationHandle();
    }

    public IDistributedSynchronizationHandle Acquire(TimeSpan? timeout = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return new NoopDistributedSynchronizationHandle();
    }

    public ValueTask<IDistributedSynchronizationHandle?> TryAcquireAsync(TimeSpan timeout = new TimeSpan(), CancellationToken cancellationToken = new CancellationToken())
    {
        return new ValueTask<IDistributedSynchronizationHandle?>(new NoopDistributedSynchronizationHandle());
    }

    public ValueTask<IDistributedSynchronizationHandle> AcquireAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return new ValueTask<IDistributedSynchronizationHandle>(new NoopDistributedSynchronizationHandle());
    }

    public string Name => "Noop";
}