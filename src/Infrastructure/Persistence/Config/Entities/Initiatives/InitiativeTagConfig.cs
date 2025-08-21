namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative Tag entity configuration.
/// </summary>
public class InitiativeTagConfig : IEntityTypeConfiguration<InitiativeTag>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<InitiativeTag> builder)
    {
        builder.ToTable("initiative_tag", "initiatives");

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

        builder.Property(i => i.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder.HasOne(e => e.Category)
            .WithMany(p => p.InitiativeTags)
            .HasForeignKey(e => e.CategoryId);

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
