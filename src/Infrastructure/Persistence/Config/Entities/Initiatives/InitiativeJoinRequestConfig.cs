namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Initiative Join Request entity configuration.
/// </summary>
public class InitiativeJoinRequestConfig : IEntityTypeConfiguration<InitiativeJoinRequest>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<InitiativeJoinRequest> builder)
    {
        builder.ToTable("initiative_join_request", "initiatives");

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

        builder.Property(i => i.StatusId)
            .HasColumnName("status_id")
            .IsRequired();

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.InitiativeJoinRequests)
            .HasForeignKey(e => e.InitiativeId);

        builder.HasOne(e => e.Status)
            .WithMany(p => p.InitiativeJoinRequests)
            .HasForeignKey(e => e.StatusId);
    }
}
