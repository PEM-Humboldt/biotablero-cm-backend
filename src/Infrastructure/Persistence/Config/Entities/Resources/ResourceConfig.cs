namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Resources;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Resource entity configuration.
/// </summary>
public class ResourceConfig : IEntityTypeConfiguration<Resource>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("resource", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder.Property(i => i.ResourceTypeId)
            .HasColumnName("resource_type_id")
            .IsRequired();

        builder.Property(i => i.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(i => i.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(e => e.PublicationDate)
            .HasColumnName("publication_date");

        builder.Property(i => i.IsDraft)
            .HasColumnName("is_draft")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Ignore(i => i.TotalLikes);

        builder.Ignore(i => i.ILikedIt);

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.Resources)
            .HasForeignKey(e => e.InitiativeId);

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
