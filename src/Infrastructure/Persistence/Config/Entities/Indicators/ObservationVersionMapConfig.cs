namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Observation Version Map entity configuration.
/// </summary>
public class ObservationVersionMapConfig : IEntityTypeConfiguration<ObservationVersionMap>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ObservationVersionMap> builder)
    {
        builder?.ToTable("indicator_version_map", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.ObservationVersionId)
            .HasColumnName("indicator_version_id")
            .IsRequired();

        builder?.Property(e => e.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();

        builder?.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder?.Property(e => e.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(1000)
            .IsRequired();

        builder?.HasOne(e => e.ObservationVersion)
            .WithMany(p => p.Maps)
            .HasForeignKey(e => e.ObservationVersionId);

        builder?
            .HasIndex(e => new { e.ObservationVersionId, e.Title })
            .IsUnique();
    }
}
