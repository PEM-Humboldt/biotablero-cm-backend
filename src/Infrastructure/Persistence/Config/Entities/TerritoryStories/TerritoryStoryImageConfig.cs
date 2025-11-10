namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.TerritoryStories;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Territory Story Image entity configuration.
/// </summary>
public class TerritoryStoryImageConfig : IEntityTypeConfiguration<TerritoryStoryImage>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<TerritoryStoryImage> builder)
    {
        builder.ToTable("territory_story_image", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.TerritoryStoryId)
            .HasColumnName("territory_story_id")
            .IsRequired();

        builder.Property(i => i.FileUrl)
            .HasColumnName("file_url")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(i => i.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(i => i.FeaturedContent)
            .HasColumnName("featured_content")
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(e => e.TerritoryStory)
            .WithMany(p => p.Images)
            .HasForeignKey(e => e.TerritoryStoryId);

        builder
            .HasIndex(e => e.FileUrl)
            .IsUnique();
    }
}
