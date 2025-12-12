namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Resources;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Resource File entity configuration.
/// </summary>
public class ResourceFileConfig : IEntityTypeConfiguration<ResourceFile>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ResourceFile> builder)
    {
        builder.ToTable("resource_file", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(i => i.Url)
            .HasColumnName("url")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(i => i.ResourceId)
            .HasColumnName("resource_id")
            .IsRequired();

        builder.HasOne(e => e.Resource)
            .WithMany(p => p.Files)
            .HasForeignKey(e => e.ResourceId);

        builder
            .HasIndex(e => new { e.ResourceId, e.Url })
            .IsUnique();
    }
}
