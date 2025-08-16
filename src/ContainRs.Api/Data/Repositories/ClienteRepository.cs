using ContainRs.Api.Contracts;
using ContainRs.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ContainRs.Api.Data.Repositories;

public class ClienteRepository(AppDbContext dbContext) : IRepository<Cliente>
{
    public async Task<Cliente> AddAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await dbContext.Clientes.AddAsync(cliente, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return cliente;
    }

    public async Task<IEnumerable<Cliente>> GetWhereAsync(Expression<Func<Cliente, bool>>? filtro = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Cliente> queryClientes = dbContext.Clientes;
        if (filtro != null)
        {
            queryClientes = queryClientes.Where(filtro);
        }
        return await queryClientes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Cliente?> GetFirstAsync<TProperty>(Expression<Func<Cliente, bool>> filtro, Expression<Func<Cliente, TProperty>> orderBy, CancellationToken cancellationToken = default)
    {
        return await dbContext.Clientes
            .Include(c => c.Enderecos)
            .AsNoTracking()
            .OrderBy(orderBy)
            .FirstOrDefaultAsync(filtro, cancellationToken);
    }

    public async Task RemoveAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        dbContext.Clientes.Remove(cliente);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Cliente> UpdateAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        // verificando endereços a serem removidos
        dbContext.Set<Endereco>()
            .Where(e => e.ClienteId == cliente.Id && !cliente.Enderecos.Contains(e))
            .ToList()
            .ForEach(e => dbContext.Set<Endereco>().Remove(e));

        dbContext.Clientes.Update(cliente);
        await dbContext.SaveChangesAsync(cancellationToken);
        return cliente;
    }
}
