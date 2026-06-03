namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Indicator Group entity configuration.
/// </summary>
public class IndicatorGroupConfig : IEntityTypeConfiguration<IndicatorGroup>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IndicatorGroup> builder)
    {
        builder.ToTable("indicator_group", "indicators");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.IndicatorVersionId)
            .HasColumnName("indicator_version_id")
            .IsRequired();

        builder.Property(i => i.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder.HasOne(e => e.IndicatorVersion)
            .WithMany(p => p.Groups)
            .HasForeignKey(e => e.IndicatorVersionId);

        builder.HasOne(e => e.Category)
            .WithMany(p => p.IndicatorGroups)
            .HasForeignKey(e => e.CategoryId);

        builder
            .HasIndex(e => new { e.IndicatorVersionId, e.CategoryId })
            .IsUnique();
    }
}
