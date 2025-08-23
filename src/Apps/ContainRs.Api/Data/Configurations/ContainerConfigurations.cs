using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainRs.Api.Data.Configurations
{
    public class ConteinerConfigurations : IEntityTypeConfiguration<Conteiner>
    {
        public void Configure(EntityTypeBuilder<Conteiner> builder)
        {
            builder
                .Property(c => c.Status)
                .HasConversion<string>();
        }
    }
}
