namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.Entities.Logging;

using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Log entity configuration.
/// </summary>
public class LogConfig : IEntityTypeConfiguration<LogEntity>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(EntityTypeBuilder<LogEntity> builder)
    {
        builder.ToTable("logs", "logs");

        builder?.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.TimeStamp)
            .HasColumnName("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(e => e.Level)
            .HasColumnName("level")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasDefaultValue(LogType.System)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(e => e.ShortMessage)
            .HasColumnName("short_message")
            .HasColumnType("text");

        builder.Property(e => e.Message)
            .HasColumnName("message")
            .HasColumnType("text");

        builder.Property(e => e.UserName)
            .HasColumnName("user_name")
            .HasColumnType("text");

        builder.Property(e => e.CustomRecord)
            .HasColumnName("custom_record")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.ClientIp)
            .HasColumnName("client_ip")
            .HasColumnType("text");

        builder.Property(e => e.ClientAgent)
            .HasColumnName("client_agent")
            .HasColumnType("text");

        builder.Property(e => e.Properties)
            .HasColumnName("properties")
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
