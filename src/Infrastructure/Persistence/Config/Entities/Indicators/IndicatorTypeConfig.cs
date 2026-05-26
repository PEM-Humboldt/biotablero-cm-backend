namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Indicator Type entity configuration.
/// </summary>
public class IndicatorTypeConfig : IEntityTypeConfiguration<IndicatorType>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IndicatorType> builder)
    {
        builder.ToTable("indicator_type", "indicators");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
