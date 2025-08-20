namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative entity configuration.
/// </summary>
public class InitiativeConfig : IEntityTypeConfiguration<Initiative>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<Initiative> builder)
    {
        builder.ToTable("initiative", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(i => i.Description)
            .HasColumnName("description")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(i => i.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(150);

        builder.Property(i => i.BannerUrl)
            .HasColumnName("banner_url")
            .HasMaxLength(150);

        builder.Property(lp => lp.Polygon)
            .HasColumnName("polygon")
            .HasColumnType("geometry(Polygon, 4326)");

        builder.Property(lp => lp.Coordinate)
            .HasColumnName("coordinate")
            .HasColumnType("geometry(Point, 4326)")
            .IsRequired();

        builder.Property(i => i.Enabled)
            .HasColumnName("enabled")
            .HasDefaultValue(true)
            .IsRequired();
    }
}
