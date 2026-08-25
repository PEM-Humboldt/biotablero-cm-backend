namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Indicator entity configuration.
/// </summary>
public class IndicatorConfig : IEntityTypeConfiguration<Indicator>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Indicator> builder)
    {
        builder?.ToTable("indicator", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(240)
            .IsRequired();

        builder?.Property(e => e.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder?.Property(e => e.IndicatorTypeId)
            .HasColumnName("indicator_type_id")
            .IsRequired();

        builder?.HasOne(e => e.Initiative)
            .WithMany(p => p.Indicators)
            .HasForeignKey(e => e.InitiativeId);

        builder?.HasOne(e => e.Type)
            .WithMany(p => p.Indicators)
            .HasForeignKey(e => e.IndicatorTypeId);
    }
}
