using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainRs.Api.Data.Configurations;

public class EnderecoConfigurations : IEntityTypeConfiguration<EnderecoCliente>
{
    public void Configure(EntityTypeBuilder<EnderecoCliente> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CEP).IsRequired();
        builder.Property(e => e.Estado).HasConversion<string>();
        builder.HasOne(e => e.Cliente)
            .WithMany(c => c.Enderecos)
            .HasForeignKey(e => e.ClienteId);
    }
}
