namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Tag entity configuration.
/// </summary>
public class TagConfig : IEntityTypeConfiguration<Tag>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tag", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.Name)
            .HasColumnName("name")
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(i => i.Url)
            .HasColumnName("url")
            .HasMaxLength(150);

        builder.Property(i => i.TagCategoryId)
            .HasColumnName("tag_category_id")
            .IsRequired();

        builder.HasOne(e => e.TagCategory)
            .WithMany(p => p.Tags)
            .HasForeignKey(e => e.TagCategoryId);

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
