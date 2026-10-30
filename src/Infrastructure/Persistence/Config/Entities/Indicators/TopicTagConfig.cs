namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Topic Tag entity configuration.
/// </summary>
public class TopicTagConfig : IEntityTypeConfiguration<TopicTag>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TopicTag> builder)
    {
        builder?.ToTable("topic_tag", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.TagId)
            .HasColumnName("tag_id")
            .IsRequired();

        builder?.Property(e => e.TopicId)
            .HasColumnName("topic_id")
            .IsRequired();

        builder?.HasOne(e => e.Tag)
            .WithMany(p => p.TopicTags)
            .HasForeignKey(e => e.TagId);

        builder?.HasOne(e => e.Topic)
            .WithMany(p => p.TopicTags)
            .HasForeignKey(e => e.TopicId);

        builder?
            .HasIndex(e => new { e.TopicId, e.TagId })
            .IsUnique();
    }
}
