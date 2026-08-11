namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Resources;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Resource Type entity configuration.
/// </summary>
public class ResourceTypeConfig : IEntityTypeConfiguration<ResourceType>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ResourceType> builder)
    {
        builder?.ToTable("resource_type", "initiatives");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder?.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder?
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
