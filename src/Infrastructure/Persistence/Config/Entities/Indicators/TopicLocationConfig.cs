namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Topic Location entity configuration.
/// </summary>
public class TopicLocationConfig : IEntityTypeConfiguration<TopicLocation>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TopicLocation> builder)
    {
        builder?.ToTable("topic_location", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.TopicId)
            .HasColumnName("topic_id")
            .IsRequired();

        builder?.Property(e => e.LocationId)
            .HasColumnName("location_id")
            .IsRequired();

        builder?.Property(e => e.Locality)
            .HasColumnName("locality")
            .HasMaxLength(300);

        builder?.HasOne(e => e.Topic)
            .WithMany(p => p.TopicLocations)
            .HasForeignKey(e => e.TopicId);

        builder?.HasOne(e => e.Location)
            .WithMany(p => p.TopicLocations)
            .HasForeignKey(e => e.LocationId);

        builder?
            .HasIndex(e => new { e.TopicId, e.LocationId, e.Locality })
            .IsUnique();
    }
}
