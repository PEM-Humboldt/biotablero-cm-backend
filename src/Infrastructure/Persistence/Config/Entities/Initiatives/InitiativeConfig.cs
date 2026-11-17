namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative entity configuration.
/// </summary>
public class InitiativeConfig : IEntityTypeConfiguration<Initiative>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Initiative> builder)
    {
        builder?.ToTable("initiative", "initiatives");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder?.Property(e => e.ShortName)
            .HasColumnName("short_name")
            .HasMaxLength(120);

        builder?.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsRequired();

        builder?.Property(e => e.Baseline)
            .HasColumnName("baseline")
            .HasMaxLength(1000);

        builder?.Property(e => e.Objective)
            .HasColumnName("objective")
            .HasMaxLength(1000);

        builder?.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder?.Property(e => e.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(150);

        builder?.Property(e => e.BannerUrl)
            .HasColumnName("banner_url")
            .HasMaxLength(150);

        builder?.Property(lp => lp.Polygon)
            .HasColumnName("polygon")
            .HasColumnType("geometry(Polygon, 4326)");

        builder?.Property(lp => lp.Coordinate)
            .HasColumnName("coordinate")
            .HasColumnType("geometry(Point, 4326)")
            .HasDefaultValueSql("ST_GeomFromText('POINT EMPTY', 4326)")
            .IsRequired();

        builder?.Property(e => e.PolygonArea)
            .HasColumnName("polygon_area")
            .HasDefaultValue(0m)
            .IsRequired();

        builder?.Property(e => e.MainLocationId)
            .HasColumnName("main_location_id")
            .HasDefaultValue(0)
            .IsRequired();

        builder?.Property(e => e.Enabled)
            .HasColumnName("enabled")
            .HasDefaultValue(true)
            .IsRequired();

        builder?.HasOne(l => l.MainLocation)
            .WithMany(p => p.Initiatives)
            .HasForeignKey(l => l.MainLocationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
