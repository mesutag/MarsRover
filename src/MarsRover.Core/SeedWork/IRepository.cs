namespace MarsRover.Core.SeedWork
{
    public interface IRepository<TEntity> where TEntity : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get;}
    }
}
