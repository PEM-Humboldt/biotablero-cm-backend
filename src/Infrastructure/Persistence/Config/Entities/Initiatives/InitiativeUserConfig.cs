namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative User entity configuration.
/// </summary>
public class InitiativeUserConfig : IEntityTypeConfiguration<InitiativeUser>
{
    /// <inheritdoc/>
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

        builder.Property(i => i.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(i => i.FocusArea)
            .HasColumnName("focus_area")
            .HasMaxLength(200);

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(i => i.LevelId)
            .HasColumnName("initiative_user_level_id")
            .IsRequired();

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.InitiativeUsers)
            .HasForeignKey(e => e.InitiativeId);

        builder.HasOne(e => e.Level)
            .WithMany(p => p.InitiativeUsers)
            .HasForeignKey(e => e.LevelId);

        builder
            .HasIndex(e => new { e.UserName, e.InitiativeId })
            .IsUnique();
    }
}
