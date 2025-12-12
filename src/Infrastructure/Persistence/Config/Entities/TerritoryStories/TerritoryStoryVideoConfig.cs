namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.TerritoryStories;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Territory Story Video entity configuration.
/// </summary>
public class TerritoryStoryVideoConfig : IEntityTypeConfiguration<TerritoryStoryVideo>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TerritoryStoryVideo> builder)
    {
        builder.ToTable("territory_story_video", "initiatives");

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

        builder.HasOne(e => e.TerritoryStory)
            .WithMany(p => p.Videos)
            .HasForeignKey(e => e.TerritoryStoryId);

        builder
            .HasIndex(e => e.FileUrl)
            .IsUnique();
    }
}
