using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Core.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
