namespace IAVH.BioTablero.CM.Persistence.Config;

using IAVH.BioTablero.CM.Core.Entities.LogNS;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfig : IEntityTypeConfiguration<LogEntity>
{
    public void Configure(EntityTypeBuilder<LogEntity> builder)
    {
        builder.ToTable("logs", "logs");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

    }
}
