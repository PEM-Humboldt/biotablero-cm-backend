namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Category entity configuration.
/// </summary>
public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder?.ToTable("category", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.ParentId)
            .HasColumnName("parent_id");

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(70)
            .IsRequired();

        builder?.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(240);

        builder?.HasOne(e => e.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey(e => e.ParentId);

        builder?
            .HasIndex(e => new { e.ParentId, e.Name })
            .IsUnique();
    }
}
