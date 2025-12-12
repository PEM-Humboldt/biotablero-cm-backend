namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Resources;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Resource Like entity configuration.
/// </summary>
public class ResourceLikeConfig : IEntityTypeConfiguration<ResourceLike>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ResourceLike> builder)
    {
        builder.ToTable("resource_like", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.ResourceId)
            .HasColumnName("resource_id")
            .IsRequired();

        builder.Property(i => i.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(e => e.Resource)
            .WithMany(p => p.Likes)
            .HasForeignKey(e => e.ResourceId);

        builder
            .HasIndex(e => new { e.ResourceId, e.UserName })
            .IsUnique();
    }
}
