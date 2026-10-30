namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Topic Type entity configuration.
/// </summary>
public class TopicTypeConfig : IEntityTypeConfiguration<TopicType>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TopicType> builder)
    {
        builder?.ToTable("topic_type", "indicators");

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
