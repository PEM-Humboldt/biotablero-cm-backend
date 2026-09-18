namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Map Legend entity configuration.
/// </summary>
public class MapLegendConfig : IEntityTypeConfiguration<MapLegend>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<MapLegend> builder)
    {
        builder?.ToTable("map_legend", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.ObservationVersionMapId)
            .HasColumnName("indicator_version_map_id")
            .IsRequired();

        builder?.Property(e => e.Title)
            .HasColumnName("title")
            .HasMaxLength(50)
            .IsRequired();

        builder?.HasOne(e => e.ObservationVersionMap)
            .WithMany(p => p.Legends)
            .HasForeignKey(e => e.ObservationVersionMapId);

        builder?
            .HasIndex(e => new { e.ObservationVersionMapId, e.Title })
            .IsUnique();
    }
}
