using ContainRs.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainRs.Api.Data.Configurations;

public class PropostaConfigurations : IEntityTypeConfiguration<Proposta>
{
    public void Configure(EntityTypeBuilder<Proposta> builder)
    {
        builder.Property(p => p.ValorTotal)
            .HasColumnType("decimal(18,2)");

        builder.OwnsOne(p => p.Status, status =>
        {
            status.Property(s => s.Status)
                .HasColumnName("Status")
                .HasConversion<string>();
        });

        builder.HasOne(p => p.Solicitacao)
            .WithMany(s => s.Propostas)
            .HasForeignKey(p => p.SolicitacaoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
