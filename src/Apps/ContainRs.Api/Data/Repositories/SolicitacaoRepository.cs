using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ContainRs.Api.Data.Repositories;

public class SolicitacaoRepository(AppDbContext dbContext) 
    : BaseRepository<PedidoLocacao>(dbContext)
{
    // métodos sobrescritos e específicos vão aqui
    public override Task<PedidoLocacao?> GetFirstAsync<TProperty>(Expression<Func<PedidoLocacao, bool>> filtro, Expression<Func<PedidoLocacao, TProperty>> orderBy, CancellationToken cancellationToken = default)
    {
        return dbContext.Pedidos
            .Include(s => s.Propostas)
            .AsNoTracking()
            .OrderBy(orderBy)
            .FirstOrDefaultAsync(filtro, cancellationToken);
    }
}
