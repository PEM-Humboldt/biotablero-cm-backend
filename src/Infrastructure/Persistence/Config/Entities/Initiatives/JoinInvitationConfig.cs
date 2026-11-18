namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Join Invitation entity configuration.
/// </summary>
public class JoinInvitationConfig : IEntityTypeConfiguration<JoinInvitation>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<JoinInvitation> builder)
    {
        builder?.ToTable("join_invitation", "initiatives");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder?.Property(e => e.Creator)
            .HasColumnName("creator_user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder?.Property(e => e.Message)
            .HasColumnName("message")
            .HasMaxLength(350);

        builder?.Property(e => e.HtmlMessage)
            .HasColumnName("html_message")
            .IsRequired();

        builder?.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder?.Ignore(i => i.CreatorFullName);

        builder?.HasOne(e => e.Initiative)
            .WithMany(p => p.JoinInvitations)
            .HasForeignKey(e => e.InitiativeId);
    }
}
