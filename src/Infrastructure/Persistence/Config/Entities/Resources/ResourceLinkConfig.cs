namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Resources;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Resource Link entity configuration.
/// </summary>
public class ResourceLinkConfig : IEntityTypeConfiguration<ResourceLink>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ResourceLink> builder)
    {
        builder?.ToTable("resource_link", "initiatives");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder?.Property(e => e.Url)
            .HasColumnName("url")
            .HasMaxLength(250)
            .IsRequired();

        builder?.Property(e => e.ResourceId)
            .HasColumnName("resource_id")
            .IsRequired();

        builder?.HasOne(e => e.Resource)
            .WithMany(p => p.Links)
            .HasForeignKey(e => e.ResourceId);

        builder?
            .HasIndex(e => new { e.ResourceId, e.Url })
            .IsUnique();
    }
}
