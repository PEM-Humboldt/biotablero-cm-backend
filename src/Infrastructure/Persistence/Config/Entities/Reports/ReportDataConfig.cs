namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Reports;

using IAVH.BioTablero.CM.Core.Domain.Entities.Reports;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Report Data entity configuration.
/// </summary>
public class ReportDataConfig : IEntityTypeConfiguration<ReportData>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ReportData> builder)
    {
        builder?.ToTable("report_data", "reports");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder?.Property(e => e.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(75)
            .IsRequired();

        builder?.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder?.Property(e => e.Data)
            .HasColumnName("data")
            .HasMaxLength(280);
    }
}
