using ContainRs.Api.Domain;

namespace ContainRs.Api.Data.Repositories;

public class LocacaoRepository(AppDbContext context) : BaseRepository<Locacao>(context)
{
}
