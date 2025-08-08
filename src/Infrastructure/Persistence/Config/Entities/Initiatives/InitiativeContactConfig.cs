namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative Contact entity configuration.
/// </summary>
public class InitiativeContactConfig : IEntityTypeConfiguration<InitiativeContact>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<InitiativeContact> builder)
    {
        builder.ToTable("initiative_contact", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder.Property(i => i.Phone)
            .HasColumnName("phone");

        builder.Property(i => i.Email)
            .HasColumnName("email")
            .IsRequired();

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.InitiativeContacts)
            .HasForeignKey(e => e.InitiativeId);

        builder
            .HasIndex(e => new { e.InitiativeId, e.Email })
            .IsUnique();

        builder
            .HasIndex(e => new { e.InitiativeId, e.Phone })
            .IsUnique();
    }
}
