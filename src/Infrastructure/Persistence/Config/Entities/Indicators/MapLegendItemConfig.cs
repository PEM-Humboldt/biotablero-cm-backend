namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Map Legend Item entity configuration.
/// </summary>
public class MapLegendItemConfig : IEntityTypeConfiguration<MapLegendItem>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<MapLegendItem> builder)
    {
        builder.ToTable("map_legend_item", "indicators");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.MapLegendId)
            .HasColumnName("map_legend_id")
            .IsRequired();

        builder.Property(e => e.ColorCode)
            .HasColumnName("color_code")
            .HasMaxLength(6)
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnName("value")
            .HasMaxLength(10)
            .IsRequired();

        builder.HasOne(e => e.MapLegend)
            .WithMany(p => p.Items)
            .HasForeignKey(e => e.MapLegendId);

        builder
            .HasIndex(e => new { e.MapLegendId, e.ColorCode })
            .IsUnique();
    }
}
