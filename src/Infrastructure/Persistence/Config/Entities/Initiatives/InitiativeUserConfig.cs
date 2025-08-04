namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative User entity configuration
/// </summary>
public class InitiativeUserConfig : IEntityTypeConfiguration<InitiativeUser>
{
    /// <summary>
    /// Configure entity
    /// </summary>
    /// <param name="builder">Entity builder</param>
    public void Configure(EntityTypeBuilder<InitiativeUser> builder)
    {
        builder.ToTable("initiative_user", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder.Property(i => i.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(i => i.LevelId)
            .HasColumnName("level_id")
            .IsRequired();

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.InitiativeUsers)
            .HasForeignKey(e => e.InitiativeId);

        builder
            .HasIndex(e => new { e.UserId, e.InitiativeId })
            .IsUnique();
    }
}
