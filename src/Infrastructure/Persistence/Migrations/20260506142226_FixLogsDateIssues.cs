#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class FixLogsDateIssues : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "timestamp",
            schema: "logs",
            table: "logs",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<DateTime>(
            name: "timestamp",
            schema: "logs",
            table: "logs",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");
}
