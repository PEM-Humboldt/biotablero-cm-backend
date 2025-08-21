namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative Tag Initiative entity configuration.
/// </summary>
public class InitiativeTagInitiativeConfig : IEntityTypeConfiguration<InitiativeTagInitiative>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<InitiativeTagInitiative> builder)
    {
        builder.ToTable("initiative_tag_initiative", "initiatives");

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
            .WithMany(p => p.InitiativeTagInitiatives)
            .HasForeignKey(e => e.TagId);

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.InitiativeTagInitiatives)
            .HasForeignKey(e => e.InitiativeId);
    }
}
