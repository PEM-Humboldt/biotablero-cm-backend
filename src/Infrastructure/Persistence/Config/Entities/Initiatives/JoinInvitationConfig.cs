namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Join Invitation entity configuration.
/// </summary>
public class JoinInvitationConfig : IEntityTypeConfiguration<JoinInvitation>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<JoinInvitation> builder)
    {
        builder.ToTable("join_invitation", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder.Property(i => i.CreatorUserName)
            .HasColumnName("creator_user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(i => i.Message)
            .HasColumnName("message")
            .HasMaxLength(200);

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.JoinInvitations)
            .HasForeignKey(e => e.InitiativeId);
    }
}
