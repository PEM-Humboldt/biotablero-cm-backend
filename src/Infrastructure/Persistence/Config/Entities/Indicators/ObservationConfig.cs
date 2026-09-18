namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Observation entity configuration.
/// </summary>
public class ObservationConfig : IEntityTypeConfiguration<Observation>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Observation> builder)
    {
        builder?.ToTable("observation", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(500)
            .IsRequired();

        builder?.Property(e => e.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder?.Property(e => e.IndicatorTopicId)
            .HasColumnName("indicator_topic_id")
            .IsRequired();

        builder?.HasOne(e => e.Initiative)
            .WithMany(p => p.Observations)
            .HasForeignKey(e => e.InitiativeId);

        builder?.HasOne(e => e.Topic)
            .WithMany(p => p.Observations)
            .HasForeignKey(e => e.IndicatorTopicId);

        builder?
            .HasIndex(e => new { e.InitiativeId, e.Name })
            .IsUnique();
    }
}
