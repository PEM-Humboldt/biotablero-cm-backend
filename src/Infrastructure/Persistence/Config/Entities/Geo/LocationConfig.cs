namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Geo;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Location builder configuration.
/// </summary>
public class LocationConfig : IEntityTypeConfiguration<Location>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("location", "geo");

        builder?.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(l => l.Name)
            .HasColumnName("name")
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(l => l.Code)
            .HasColumnName("code")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(l => l.Level)
            .HasColumnName("level")
            .IsRequired();

        builder.Property(l => l.ParentId)
            .HasColumnName("parent_id");

        builder.HasIndex(u => u.Code)
            .IsUnique();

        builder.HasOne(l => l.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey(l => l.ParentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
