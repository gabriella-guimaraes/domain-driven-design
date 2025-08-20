using ContainRs.Api.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ContainRs.Api.Data.Repositories;

public abstract class BaseRepository<T>(AppDbContext dbContext) 
    : IRepository<T> where T : class
{
    public virtual async Task<T> AddAsync(T obj, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<T>().AddAsync(obj, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return obj;
    }

    public virtual async Task<T?> GetFirstAsync<TProperty>(Expression<Func<T, bool>> filtro, Expression<Func<T, TProperty>> orderBy, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<T>()
            .AsNoTracking()
            .OrderBy(orderBy)
            .FirstOrDefaultAsync(filtro, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>>? filtro = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = dbContext.Set<T>();
        if (filtro != null)
        {
            query = query.Where(filtro);
        }
        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task RemoveAsync(T obj, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().Remove(obj);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<T> UpdateAsync(T obj, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().Update(obj);
        await dbContext.SaveChangesAsync(cancellationToken);
        return obj;
    }
}
