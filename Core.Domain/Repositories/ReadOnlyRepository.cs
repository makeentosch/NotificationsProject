using System.Linq.Expressions;
using Core.Domain.Repositories.Interfaces;

namespace Core.Domain.Repositories;

public abstract class ReadOnlyRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
{
    public virtual bool IsReadOnly { get; set; }
    public abstract ValueTask<TAggregateRoot?> FindAsync(object[] keyValues, CancellationToken cancellationToken);
    public abstract Task<IReadOnlyList<TAggregateRoot>> ListAsync(CancellationToken cancellationToken);
    public abstract Task<IReadOnlyList<TAggregateRoot>> ListAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
    public abstract Task<TAggregateRoot> SingleAsync(CancellationToken cancellationToken);
    public abstract Task<TAggregateRoot> SingleAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
    public abstract Task<TAggregateRoot?> SingleOrDefaultAsync(CancellationToken cancellationToken);
    public abstract Task<TAggregateRoot?> SingleOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
    public abstract Task<TAggregateRoot> FirstAsync(CancellationToken cancellationToken);
    public abstract Task<TAggregateRoot> FirstAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
    public abstract Task<TAggregateRoot?> FirstOrDefaultAsync(CancellationToken cancellationToken);
    public abstract Task<TAggregateRoot?> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
    public abstract Task<int> CountAsync(CancellationToken cancellationToken);
    public abstract Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
    public abstract Task<long> LongCountAsync(CancellationToken cancellationToken);
    public abstract Task<long> LongCountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);

    public abstract Task<IReadOnlyList<TResult>> QueryAsync<TResult>(
        Func<IQueryable<TAggregateRoot>, IQueryable<TResult>> predicate, CancellationToken cancellationToken);
}