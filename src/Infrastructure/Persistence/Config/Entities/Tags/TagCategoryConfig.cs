namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Tags;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Tag Category entity configuration.
/// </summary>
public class TagCategoryConfig : IEntityTypeConfiguration<TagCategory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TagCategory> builder)
    {
        builder.ToTable("tag_category", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.Name)
            .HasColumnName("name")
            .HasMaxLength(30)
            .IsRequired();

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
