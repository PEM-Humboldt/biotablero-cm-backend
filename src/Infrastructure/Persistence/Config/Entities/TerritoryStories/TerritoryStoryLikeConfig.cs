namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.TerritoryStories;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Territory Story Like entity configuration.
/// </summary>
public class TerritoryStoryLikeConfig : IEntityTypeConfiguration<TerritoryStoryLike>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<TerritoryStoryLike> builder)
    {
        builder.ToTable("territory_story_like", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.TerritoryStoryId)
            .HasColumnName("territory_story_id")
            .IsRequired();

        builder.Property(i => i.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(e => e.TerritoryStory)
            .WithMany(p => p.Likes)
            .HasForeignKey(e => e.TerritoryStoryId);

        builder
            .HasIndex(e => new { e.TerritoryStoryId, e.UserName })
            .IsUnique();
    }
}
