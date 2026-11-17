namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Notifications;

using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Notification entity configuration.
/// </summary>
public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder?.ToTable("notification", "notifications");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.Receiver)
            .HasColumnName("receiver")
            .HasMaxLength(75)
            .IsRequired();

        builder?.Property(e => e.Subject)
            .HasColumnName("subject")
            .HasMaxLength(120)
            .IsRequired();

        builder?.Property(e => e.Body)
            .HasColumnName("body")
            .HasMaxLength(2000)
            .IsRequired();

        builder?.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder?.Property(e => e.ReadingDate)
            .HasColumnName("reading_date");

        builder?.Property(e => e.IsRead)
            .HasColumnName("is_read")
            .HasDefaultValue(false)
            .IsRequired();

        builder?.Property(e => e.Properties)
            .HasColumnName("properties")
            .HasColumnType("jsonb");
    }
}
