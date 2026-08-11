namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Resources;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Resource Tag entity configuration.
/// </summary>
public class ResourceTagConfig : IEntityTypeConfiguration<ResourceTag>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ResourceTag> builder)
    {
        builder?.ToTable("resource_tag", "initiatives");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.TagId)
            .HasColumnName("tag_id")
            .IsRequired();

        builder?.Property(e => e.ResourceId)
            .HasColumnName("resource_id")
            .IsRequired();

        builder?.HasOne(e => e.Tag)
            .WithMany(p => p.TagResources)
            .HasForeignKey(e => e.TagId);

        builder?.HasOne(e => e.Resource)
            .WithMany(p => p.ResourceTags)
            .HasForeignKey(e => e.ResourceId);

        builder?
            .HasIndex(e => new { e.ResourceId, e.TagId })
            .IsUnique();
    }
}
