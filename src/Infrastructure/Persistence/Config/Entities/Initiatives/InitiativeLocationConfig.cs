namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative Location entity configuration
/// </summary>
public class InitiativeLocationConfig : IEntityTypeConfiguration<InitiativeLocation>
{
    /// <summary>
    /// Configure entity
    /// </summary>
    /// <param name="builder">Entity builder</param>
    public void Configure(EntityTypeBuilder<InitiativeLocation> builder)
    {
        builder.ToTable("initiative_location", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder.Property(i => i.LocationId)
            .HasColumnName("location_id")
            .IsRequired();

        builder.Property(i => i.Locality)
            .HasColumnName("locality");

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.InitiativeLocations)
            .HasForeignKey(e => e.InitiativeId);

        builder.HasOne(e => e.Location)
            .WithMany(p => p.InitiativeLocations)
            .HasForeignKey(e => e.InitiativeId);

        builder
            .HasIndex(e => new { e.InitiativeId, e.LocationId })
            .IsUnique();
    }
}
