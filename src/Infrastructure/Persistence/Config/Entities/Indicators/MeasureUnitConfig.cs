namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Measure Unit entity configuration.
/// </summary>
public class MeasureUnitConfig : IEntityTypeConfiguration<MeasureUnit>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<MeasureUnit> builder)
    {
        builder.ToTable("measure_unit", "indicators");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.Name)
            .HasColumnName("name")
            .HasMaxLength(70)
            .IsRequired();

        builder.Property(i => i.Representation)
            .HasColumnName("representation")
            .HasMaxLength(10);

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
