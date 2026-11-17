namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.TerritoryStories;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Territory Story entity configuration.
/// </summary>
public class TerritoryStoryConfig : IEntityTypeConfiguration<TerritoryStory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TerritoryStory> builder)
    {
        builder?.ToTable("territory_story", "initiatives");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder?.Property(e => e.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();

        builder?.Property(e => e.Text)
            .HasColumnName("text")
            .HasMaxLength(5000);

        builder?.Property(e => e.Keywords)
            .HasColumnName("keywords")
            .HasMaxLength(75);

        builder?.Property(e => e.AuthorUserName)
            .HasColumnName("author_user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder?.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder?.Property(e => e.Restricted)
            .HasColumnName("restricted")
            .HasDefaultValue(false)
            .IsRequired();

        builder?.Property(e => e.Enabled)
            .HasColumnName("enabled")
            .HasDefaultValue(true)
            .IsRequired();

        builder?.Property(e => e.FeaturedContent)
            .HasColumnName("featured_content")
            .HasDefaultValue(false)
            .IsRequired();

        builder?.Ignore(i => i.TotalLikes);

        builder?.Ignore(i => i.ILikedIt);

        builder?.HasOne(e => e.Initiative)
            .WithMany(p => p.TerritoryStories)
            .HasForeignKey(e => e.InitiativeId);

        builder?
            .HasIndex(e => e.Title)
            .IsUnique();
    }
}
