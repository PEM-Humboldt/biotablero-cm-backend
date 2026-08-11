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
        builder?.ToTable("measure_unit", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(70)
            .IsRequired();

        builder?.Property(e => e.Representation)
            .HasColumnName("representation")
            .HasMaxLength(10);

        builder?
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
