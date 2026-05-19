namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Indicator Tag entity configuration.
/// </summary>
public class IndicatorTagConfig : IEntityTypeConfiguration<IndicatorTag>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IndicatorTag> builder)
    {
        builder.ToTable("indicator_tag", "indicators");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.TagId)
            .HasColumnName("tag_id")
            .IsRequired();

        builder.Property(i => i.IndicatorId)
            .HasColumnName("indicator_id")
            .IsRequired();

        builder.HasOne(e => e.Tag)
            .WithMany(p => p.TagIndicators)
            .HasForeignKey(e => e.TagId);

        builder.HasOne(e => e.Indicator)
            .WithMany(p => p.IndicatorTags)
            .HasForeignKey(e => e.IndicatorId);

        builder
            .HasIndex(e => new { e.IndicatorId, e.TagId })
            .IsUnique();
    }
}
