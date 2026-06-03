namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Monitoring Events entity configuration.
/// </summary>
public class MonitoringEventsConfig : IEntityTypeConfiguration<MonitoringEvents>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<MonitoringEvents> builder)
    {
        builder.ToTable("monitoring_events", "initiatives");

        builder?.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(i => i.InitiativeId)
            .HasColumnName("initiative_id")
            .IsRequired();

        builder.Property(e => e.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnName("value")
            .IsRequired();

        builder.HasOne(e => e.Initiative)
            .WithMany(p => p.MonitoringEventsList)
            .HasForeignKey(e => e.InitiativeId);

        builder
            .HasIndex(e => new { e.InitiativeId, e.Date })
            .IsUnique();
    }
}
