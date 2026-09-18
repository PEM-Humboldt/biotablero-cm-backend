namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Observation Location entity configuration.
/// </summary>
public class ObservationLocationConfig : IEntityTypeConfiguration<ObservationLocation>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ObservationLocation> builder)
    {
        builder?.ToTable("observation_location", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.ObservationId)
            .HasColumnName("observation_id")
            .IsRequired();

        builder?.Property(e => e.LocationId)
            .HasColumnName("location_id")
            .IsRequired();

        builder?.Property(e => e.Locality)
            .HasColumnName("locality")
            .HasMaxLength(300);

        builder?.HasOne(e => e.Observation)
            .WithMany(p => p.ObservationLocations)
            .HasForeignKey(e => e.ObservationId);

        builder?.HasOne(e => e.Location)
            .WithMany(p => p.ObservationLocations)
            .HasForeignKey(e => e.LocationId);

        builder?
            .HasIndex(e => new { e.ObservationId, e.LocationId, e.Locality })
            .IsUnique();
    }
}
