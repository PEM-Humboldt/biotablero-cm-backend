namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Observation Group entity configuration.
/// </summary>
public class ObservationGroupConfig : IEntityTypeConfiguration<ObservationGroup>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ObservationGroup> builder)
    {
        builder?.ToTable("observation_group", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.ObservationVersionId)
            .HasColumnName("observation_version_id")
            .IsRequired();

        builder?.Property(e => e.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder?.HasOne(e => e.ObservationVersion)
            .WithMany(p => p.Groups)
            .HasForeignKey(e => e.ObservationVersionId);

        builder?.HasOne(e => e.Category)
            .WithMany(p => p.ObservationGroups)
            .HasForeignKey(e => e.CategoryId);

        builder?
            .HasIndex(e => new { e.ObservationVersionId, e.CategoryId })
            .IsUnique();
    }
}
