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
        builder.ToTable("territory_story", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder.Property(i => i.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(i => i.Text)
            .HasColumnName("text")
            .HasMaxLength(2000);

        builder.Property(i => i.Keywords)
            .HasColumnName("keywords")
            .HasMaxLength(75);

        builder.Property(i => i.AuthorUserName)
            .HasColumnName("author_user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(i => i.Restricted)
            .HasColumnName("restricted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(i => i.Enabled)
            .HasColumnName("enabled")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(i => i.FeaturedContent)
            .HasColumnName("featured_content")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Ignore(i => i.TotalLikes);

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.TerritoryStories)
            .HasForeignKey(e => e.InitiativeId);

        builder
            .HasIndex(e => e.Title)
            .IsUnique();
    }
}
