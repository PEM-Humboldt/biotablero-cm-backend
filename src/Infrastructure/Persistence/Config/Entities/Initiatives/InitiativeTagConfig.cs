namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative Tag entity configuration.
/// </summary>
public class InitiativeTagConfig : IEntityTypeConfiguration<InitiativeTag>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<InitiativeTag> builder)
    {
        builder.ToTable("initiative_tag", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.TagId)
            .HasColumnName("tag_id")
            .IsRequired();

        builder.Property(i => i.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder.HasOne(e => e.Tag)
            .WithMany(p => p.TagInitiatives)
            .HasForeignKey(e => e.TagId);

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.InitiativeTags)
            .HasForeignKey(e => e.InitiativeId);

        builder
            .HasIndex(e => new { e.InitiativeId, e.TagId })
            .IsUnique();
    }
}
