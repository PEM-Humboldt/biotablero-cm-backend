namespace IAVH.BioTablero.CM.Persistence.Config;

using IAVH.BioTablero.CM.Core.Entities.LogNS;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfig : IEntityTypeConfiguration<LogEntity>
{
    public void Configure(EntityTypeBuilder<LogEntity> builder)
    {
        builder.ToTable("logs", "logs");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.TimeStamp)
            .HasColumnName("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(e => e.Level)
            .HasColumnName("level")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Message)
            .HasColumnName("message")
            .HasColumnType("text");

        builder.Property(e => e.UserName)
            .HasColumnName("user_name")
            .HasColumnType("text");

        builder.Property(e => e.Properties)
            .HasColumnName("properties")
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
