namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Indicator Version entity configuration.
/// </summary>
public class IndicatorVersionConfig : IEntityTypeConfiguration<IndicatorVersion>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IndicatorVersion> builder)
    {
        builder.ToTable("indicator_version", "indicators");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.IndicatorId)
            .HasColumnName("indicator_id")
            .IsRequired();

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(i => i.Version)
            .HasColumnName("version")
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(i => i.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(i => i.Methodology)
            .HasColumnName("methodology")
            .HasMaxLength(1000);

        builder.Property(i => i.Interpretation)
            .HasColumnName("interpretation")
            .HasMaxLength(1000);

        builder.Property(i => i.Considerations)
            .HasColumnName("considerations")
            .HasMaxLength(1000);

        builder.Property(i => i.Authorship)
            .HasColumnName("authorship")
            .HasMaxLength(1000);

        builder.HasOne(e => e.Indicator)
            .WithMany(p => p.Versions)
            .HasForeignKey(e => e.IndicatorId);

        builder
            .HasIndex(e => new { e.IndicatorId, e.Version })
            .IsUnique();
    }
}
