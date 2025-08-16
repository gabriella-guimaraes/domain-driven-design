using ContainRs.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainRs.Api.Data.Configurations;

public class SolicitacaoConfigurations : IEntityTypeConfiguration<Solicitacao>
{
    public void Configure(EntityTypeBuilder<Solicitacao> builder)
    {
        builder.OwnsOne(s => s.Status, status =>
        {
            status.Property(s => s.Status)
                .HasColumnName("Status")
                .HasConversion<string>();
        });
    }
}
