namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Topic Version entity configuration.
/// </summary>
public class TopicVersionConfig : IEntityTypeConfiguration<TopicVersion>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TopicVersion> builder)
    {
        builder?.ToTable("topic_version", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.TopicId)
            .HasColumnName("topic_id")
            .IsRequired();

        builder?.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder?.Property(e => e.Version)
            .HasColumnName("version")
            .HasDefaultValue(1)
            .IsRequired();

        builder?.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(3000);

        builder?.Property(e => e.Methodology)
            .HasColumnName("methodology")
            .HasMaxLength(3000);

        builder?.Property(e => e.Interpretation)
            .HasColumnName("interpretation")
            .HasMaxLength(3000);

        builder?.Property(e => e.Considerations)
            .HasColumnName("considerations")
            .HasMaxLength(3000);

        builder?.Property(e => e.Authorship)
            .HasColumnName("authorship")
            .HasMaxLength(3000);

        builder?.HasOne(e => e.Topic)
            .WithMany(p => p.Versions)
            .HasForeignKey(e => e.TopicId);

        builder?.Ignore(i => i.IndicatorTopicId);
        builder?.Ignore(i => i.TopicName);

        builder?
            .HasIndex(e => new { e.TopicId, e.Version })
            .IsUnique();
    }
}
