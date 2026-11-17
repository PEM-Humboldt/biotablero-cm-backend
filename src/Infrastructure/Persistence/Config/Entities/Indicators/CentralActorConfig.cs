namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Central Actor entity configuration.
/// </summary>
public class CentralActorConfig : IEntityTypeConfiguration<CentralActor>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CentralActor> builder)
    {
        builder?.ToTable("central_actor", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(70)
            .IsRequired();

        builder?
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
