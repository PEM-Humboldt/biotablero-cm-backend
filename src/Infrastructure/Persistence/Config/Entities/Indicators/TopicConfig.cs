namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Topic entity configuration.
/// </summary>
public class TopicConfig : IEntityTypeConfiguration<Topic>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder?.ToTable("topic", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(500)
            .IsRequired();

        builder?.Property(e => e.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder?.Property(e => e.TopicTypeId)
            .HasColumnName("topic_type_id")
            .IsRequired();

        builder?.Ignore(i => i.LastVersion);

        builder?.HasOne(e => e.Initiative)
            .WithMany(p => p.Topics)
            .HasForeignKey(e => e.InitiativeId);

        builder?.HasOne(e => e.TopicType)
            .WithMany(p => p.Topics)
            .HasForeignKey(e => e.TopicTypeId);

        builder?
            .HasIndex(e => new { e.InitiativeId, e.Name })
            .IsUnique();
    }
}
