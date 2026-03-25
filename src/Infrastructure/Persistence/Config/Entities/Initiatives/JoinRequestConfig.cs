namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Join Request entity configuration.
/// </summary>
public class JoinRequestConfig : IEntityTypeConfiguration<JoinRequest>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<JoinRequest> builder)
    {
        builder.ToTable("join_request", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(i => i.ReviewerUserName)
            .HasColumnName("reviewer_user_name")
            .HasMaxLength(75);

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(e => e.ResponseDate)
            .HasColumnName("response_date");

        builder.Property(i => i.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder.Property(i => i.LevelId)
            .HasColumnName("initiative_user_level_id");

        builder.Property(i => i.StatusId)
            .HasColumnName("join_request_status_id")
            .IsRequired();

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.JoinRequests)
            .HasForeignKey(e => e.InitiativeId);

        builder.HasOne(e => e.Level)
            .WithMany(p => p.JoinRequests)
            .HasForeignKey(e => e.LevelId);

        builder.HasOne(e => e.Status)
            .WithMany(p => p.JoinRequests)
            .HasForeignKey(e => e.StatusId);
    }
}
