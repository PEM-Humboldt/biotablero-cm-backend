namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Join Invitation Guest entity configuration.
/// </summary>
public class JoinInvitationGuestConfig : IEntityTypeConfiguration<JoinInvitationGuest>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<JoinInvitationGuest> builder)
    {
        builder.ToTable("join_invitation_guest", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.JoinInvitationId)
            .HasColumnName("join_invitation_id")
            .IsRequired();

        builder.Property(i => i.Email)
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(e => e.JoinInvitation)
            .WithMany(p => p.Guests)
            .HasForeignKey(e => e.JoinInvitationId);

        builder
            .HasIndex(e => new { e.JoinInvitationId, e.Email })
            .IsUnique();
    }
}
