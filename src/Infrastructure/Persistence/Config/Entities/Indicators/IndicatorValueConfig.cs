namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Indicator Value entity configuration.
/// </summary>
public class IndicatorValueConfig : IEntityTypeConfiguration<IndicatorValue>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IndicatorValue> builder)
    {
        builder?.ToTable("indicator_value", "indicators");

        builder?.HasKey(e => e.Id);

        builder?.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder?.Property(e => e.IndicatorGroupId)
            .HasColumnName("indicator_group_id")
            .IsRequired();

        builder?.Property(e => e.MeasureUnitId)
            .HasColumnName("measure_unit_id")
            .IsRequired();

        builder?.Property(e => e.Date)
            .HasColumnName("date")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder?.Property(e => e.DateEnd)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("date_end");

        builder?.Property(e => e.Value)
            .HasColumnName("value")
            .IsRequired();

        builder?.Property(e => e.UpperLimit)
            .HasColumnName("upper_limit");

        builder?.Property(e => e.LowerLimit)
            .HasColumnName("lower_limit");

        builder?.HasOne(e => e.Group)
            .WithMany(p => p.Values)
            .HasForeignKey(e => e.IndicatorGroupId);

        builder?.HasOne(e => e.MeasureUnit)
            .WithMany(p => p.IndicatorValues)
            .HasForeignKey(e => e.MeasureUnitId);

        builder?
            .ToTable(t =>
            t.HasCheckConstraint(
                "chk_date_end_after_date",
                "\"date_end\" IS NULL OR \"date_end\" > \"date\""));

        builder?
            .ToTable(t =>
            t.HasCheckConstraint(
                "chk_value_greater_than_lower_limit",
                "\"lower_limit\" IS NULL OR \"value\" > \"lower_limit\""));

        builder?
            .ToTable(t =>
            t.HasCheckConstraint(
                "chk_upper_limit_greater_than_value",
                "\"upper_limit\" IS NULL OR \"upper_limit\" > \"value\""));
    }
}
