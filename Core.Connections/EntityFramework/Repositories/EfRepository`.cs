using System.Linq.Expressions;
using Core.Domain.Repositories;
using Core.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Connections.EntityFramework.Repositories;

public abstract class EfRepository<TAggregateRoot, TDbContext>(TDbContext contextBase) : Repository<TAggregateRoot>(contextBase)
    where TAggregateRoot : class, IAggregateRoot
    where TDbContext : DbContext, IUnitOfWork
{
    private readonly DbContext _contextBase = contextBase;
    private DbSet<TAggregateRoot> DbSetItems => _contextBase.Set<TAggregateRoot>();
    protected virtual IQueryable<TAggregateRoot> Items => IsReadOnly ? DbSetItems.AsNoTracking() : DbSetItems;

    public override async ValueTask<TAggregateRoot> AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        var entry = await _contextBase.AddAsync(aggregateRoot, cancellationToken);
        return entry.Entity;
    }

    public override Task AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken)
    {
        return DbSetItems.AddRangeAsync(aggregateRoots, cancellationToken);
    }

    public override Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    { 
        DbSetItems.Remove(aggregateRoot);
        return Task.CompletedTask;
    }

    public override Task RemoveRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken)
    {
        DbSetItems.RemoveRange(aggregateRoots);
        return Task.CompletedTask;
    }

    public override bool IsReadOnly { get; set; }
    
    public override ValueTask<TAggregateRoot?> FindAsync(object[] keyValues, CancellationToken cancellationToken)
    {
       return DbSetItems.FindAsync(keyValues, cancellationToken);
    }

    public override async Task<IReadOnlyList<TAggregateRoot>> ListAsync(CancellationToken cancellationToken)
    {
        return await Items.ToListAsync(cancellationToken);
    }

    public override async Task<IReadOnlyList<TAggregateRoot>> ListAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Items.Where(predicate).ToListAsync(cancellationToken);
    }

    public override Task<TAggregateRoot> SingleAsync(CancellationToken cancellationToken)
    {
        return Items.SingleAsync(cancellationToken);
    }

    public override Task<TAggregateRoot> SingleAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
    {
       return Items.SingleAsync(predicate, cancellationToken);
    }

    public override Task<TAggregateRoot?> SingleOrDefaultAsync(CancellationToken cancellationToken)
    {
        return Items.SingleOrDefaultAsync(cancellationToken);
    }

    public override Task<TAggregateRoot?> SingleOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
    {
        return Items.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public override Task<TAggregateRoot> FirstAsync(CancellationToken cancellationToken)
    {
        return Items.FirstAsync(cancellationToken);
    }

    public override Task<TAggregateRoot> FirstAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
    {
        return Items.FirstAsync(predicate, cancellationToken);
    }

    public override Task<TAggregateRoot?> FirstOrDefaultAsync(CancellationToken cancellationToken)
    {
        return Items.FirstOrDefaultAsync(cancellationToken);
    }

    public override Task<TAggregateRoot?> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
    {
        return Items.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public override Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return Items.CountAsync(cancellationToken);
    }

    public override Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
    {
        return Items.CountAsync(predicate, cancellationToken);
    }

    public override Task<long> LongCountAsync(CancellationToken cancellationToken)
    {
       return Items.LongCountAsync(cancellationToken);
    }

    public override Task<long> LongCountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
    {
        return Items.LongCountAsync(predicate, cancellationToken);
    }

    public override async Task<IReadOnlyList<TResult>> QueryAsync<TResult>(Func<IQueryable<TAggregateRoot>, IQueryable<TResult>> predicate, CancellationToken cancellationToken)
    {
        return await predicate(Items).ToListAsync(cancellationToken);
    }
}