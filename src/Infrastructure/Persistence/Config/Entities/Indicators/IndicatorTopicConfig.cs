namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Indicator Topic entity configuration.
/// </summary>
public class IndicatorTopicConfig : IEntityTypeConfiguration<IndicatorTopic>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IndicatorTopic> builder)
    {
        builder?.ToTable("indicator_topic", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder?
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
