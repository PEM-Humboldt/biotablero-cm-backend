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
        builder?.ToTable("resource", "initiatives");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder?.Property(e => e.AuthorUserName)
            .HasColumnName("author_user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder?.Property(e => e.ResourceTypeId)
            .HasColumnName("resource_type_id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder?.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        builder?.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder?.Property(e => e.PublicationDate)
            .HasColumnName("publication_date");

        builder?.Property(e => e.IsDraft)
            .HasColumnName("is_draft")
            .HasDefaultValue(true)
            .IsRequired();

        builder?.Ignore(i => i.TotalLikes);

        builder?.Ignore(i => i.ILikedIt);

        builder?.Ignore(i => i.TotalFiles);

        builder?.Ignore(i => i.TotalLinks);

        builder?.HasOne(e => e.Initiative)
            .WithMany(p => p.Resources)
            .HasForeignKey(e => e.InitiativeId);

        builder?
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
