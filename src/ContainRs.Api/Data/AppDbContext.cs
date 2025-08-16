using ContainRs.Api.Domain;
using ContainRs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ContainRs.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Solicitacao> Solicitacoes { get; set; }
    public DbSet<Proposta> Propostas { get; set; }
    public DbSet<Locacao> Locacoes { get; set; }
    public DbSet<Conteiner> Conteineres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
