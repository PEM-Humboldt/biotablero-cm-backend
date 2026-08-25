namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Indicator Location entity configuration.
/// </summary>
public class IndicatorLocationConfig : IEntityTypeConfiguration<IndicatorLocation>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IndicatorLocation> builder)
    {
        builder?.ToTable("indicator_location", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.IndicatorId)
            .HasColumnName("indicator_id")
            .IsRequired();

        builder?.Property(e => e.LocationId)
            .HasColumnName("location_id")
            .IsRequired();

        builder?.Property(e => e.Locality)
            .HasColumnName("locality")
            .HasMaxLength(300);

        builder?.HasOne(e => e.Indicator)
            .WithMany(p => p.IndicatorLocations)
            .HasForeignKey(e => e.IndicatorId);

        builder?.HasOne(e => e.Location)
            .WithMany(p => p.IndicatorLocations)
            .HasForeignKey(e => e.LocationId);

        builder?
            .HasIndex(e => new { e.IndicatorId, e.LocationId, e.Locality })
            .IsUnique();
    }
}
