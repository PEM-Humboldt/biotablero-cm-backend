namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Tags;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Tag entity configuration.
/// </summary>
public class TagConfig : IEntityTypeConfiguration<Tag>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder?.ToTable("tag", "tags");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(40)
            .IsRequired();

        builder?.Property(e => e.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(120);

        builder?.Property(e => e.Url)
            .HasColumnName("url")
            .HasMaxLength(150);

        builder?.Property(e => e.CategoryId)
            .HasColumnName("tag_category_id")
            .IsRequired();

        builder?.HasOne(e => e.Category)
            .WithMany(p => p.Tags)
            .HasForeignKey(e => e.CategoryId);

        builder?
            .HasIndex(e => new { e.Name, e.CategoryId })
            .IsUnique();
    }
}
