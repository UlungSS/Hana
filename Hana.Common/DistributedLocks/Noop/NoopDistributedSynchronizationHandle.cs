using Medallion.Threading;

namespace Hana.Common.DistributedLocks.Noop;

public class NoopDistributedSynchronizationHandle : IDistributedSynchronizationHandle
{
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public ValueTask DisposeAsync()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        return new ValueTask();
    }

    public CancellationToken HandleLostToken { get; }
}