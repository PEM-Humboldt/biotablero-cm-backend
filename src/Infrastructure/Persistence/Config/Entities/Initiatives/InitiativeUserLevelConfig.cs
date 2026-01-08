namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Level entity configuration.
/// </summary>
public class InitiativeUserLevelConfig : IEntityTypeConfiguration<InitiativeUserLevel>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<InitiativeUserLevel> builder)
    {
        builder.ToTable("initiative_user_level", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(l => l.Name)
            .HasColumnName("name")
            .HasMaxLength(15)
            .IsRequired();

        builder.HasIndex(u => u.Name)
            .IsUnique();
    }
}
