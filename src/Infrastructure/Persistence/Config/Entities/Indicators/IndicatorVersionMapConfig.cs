namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Indicator Version Map entity configuration.
/// </summary>
public class IndicatorVersionMapConfig : IEntityTypeConfiguration<IndicatorVersionMap>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IndicatorVersionMap> builder)
    {
        builder.ToTable("indicator_version_map", "indicators");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.IndicatorVersionId)
            .HasColumnName("indicator_version_id")
            .IsRequired();

        builder.Property(e => e.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(e => e.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasOne(e => e.IndicatorVersion)
            .WithMany(p => p.Maps)
            .HasForeignKey(e => e.IndicatorVersionId);

        builder
            .HasIndex(e => new { e.IndicatorVersionId, e.Title })
            .IsUnique();
    }
}
