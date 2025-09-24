namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Join Request Status entity configuration.
/// </summary>
public class JoinRequestStatusConfig : IEntityTypeConfiguration<JoinRequestStatus>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<JoinRequestStatus> builder)
    {
        builder.ToTable("join_request_status", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.Name)
            .HasColumnName("name")
            .HasMaxLength(30)
            .IsRequired();

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
