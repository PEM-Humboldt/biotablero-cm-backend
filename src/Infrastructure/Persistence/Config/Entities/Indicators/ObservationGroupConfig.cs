namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Observation Group entity configuration.
/// </summary>
public class ObservationGroupConfig : IEntityTypeConfiguration<TopicGroup>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TopicGroup> builder)
    {
        builder?.ToTable("observation_group", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.TopicVersionId)
            .HasColumnName("observation_version_id")
            .IsRequired();

        builder?.Property(e => e.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder?.HasOne(e => e.TopicVersion)
            .WithMany(p => p.Groups)
            .HasForeignKey(e => e.TopicVersionId);

        builder?.HasOne(e => e.Category)
            .WithMany(p => p.ObservationGroups)
            .HasForeignKey(e => e.CategoryId);

        builder?
            .HasIndex(e => new { e.TopicVersionId, e.CategoryId })
            .IsUnique();
    }
}
