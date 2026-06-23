#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class AddIndicatorValueConstraints : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "date_end",
            schema: "indicators",
            table: "indicator_value",
            type: "timestamp without time zone",
            nullable: true,
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "date",
            schema: "indicators",
            table: "indicator_value",
            type: "timestamp without time zone",
            nullable: false,
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone");

        migrationBuilder.AddCheckConstraint(
            name: "chk_date_end_after_date",
            schema: "indicators",
            table: "indicator_value",
            sql: "\"date_end\" IS NULL OR \"date_end\" > \"date\"");

        migrationBuilder.AddCheckConstraint(
            name: "chk_upper_limit_greater_than_value",
            schema: "indicators",
            table: "indicator_value",
            sql: "\"upper_limit\" IS NULL OR \"upper_limit\" > \"value\"");

        migrationBuilder.AddCheckConstraint(
            name: "chk_value_greater_than_lower_limit",
            schema: "indicators",
            table: "indicator_value",
            sql: "\"lower_limit\" IS NULL OR \"value\" > \"lower_limit\"");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropCheckConstraint(
            name: "chk_date_end_after_date",
            schema: "indicators",
            table: "indicator_value");

        migrationBuilder.DropCheckConstraint(
            name: "chk_upper_limit_greater_than_value",
            schema: "indicators",
            table: "indicator_value");

        migrationBuilder.DropCheckConstraint(
            name: "chk_value_greater_than_lower_limit",
            schema: "indicators",
            table: "indicator_value");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "date_end",
            schema: "indicators",
            table: "indicator_value",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "date",
            schema: "indicators",
            table: "indicator_value",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone");
    }
}
