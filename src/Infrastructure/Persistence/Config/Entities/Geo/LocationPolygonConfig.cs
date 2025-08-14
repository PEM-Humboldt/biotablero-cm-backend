namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Geo;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// LocationPolygon entity configuration.
/// </summary>
public class LocationPolygonConfig : IEntityTypeConfiguration<LocationPolygon>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<LocationPolygon> builder)
    {
        builder.ToTable("location_polygons", "geo");

        builder?.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(lp => lp.Geometry)
            .HasColumnName("geometry")
            .HasColumnType("geometry(MultiPolygon, 4326)")
            .IsRequired();

        builder.Property(lp => lp.GeometrySimplified)
            .HasColumnName("geometry_simplified")
            .IsRequired();

        builder.HasOne(lp => lp.Location)
            .WithOne(l => l.LocationPolygon)
            .HasForeignKey<LocationPolygon>(lp => lp.Id)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
