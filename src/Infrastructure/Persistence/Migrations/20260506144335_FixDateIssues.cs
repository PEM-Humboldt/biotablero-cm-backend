#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class FixDateIssues : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "creation_date",
            schema: "initiatives",
            table: "territory_story_like",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "creation_date",
            schema: "initiatives",
            table: "territory_story",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "creation_date",
            schema: "initiatives",
            table: "resource_like",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "publication_date",
            schema: "initiatives",
            table: "resource",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "creation_date",
            schema: "initiatives",
            table: "resource",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "reading_date",
            schema: "notifications",
            table: "notification",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "creation_date",
            schema: "notifications",
            table: "notification",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "timestamp",
            schema: "logs",
            table: "logs",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "response_date",
            schema: "initiatives",
            table: "join_request",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "creation_date",
            schema: "initiatives",
            table: "join_request",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "creation_date",
            schema: "initiatives",
            table: "join_invitation",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "creation_date",
            schema: "initiatives",
            table: "initiative_user",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTimeOffset>(
            name: "creation_date",
            schema: "initiatives",
            table: "initiative",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "creation_date",
            schema: "initiatives",
            table: "territory_story_like",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTime>(
            name: "creation_date",
            schema: "initiatives",
            table: "territory_story",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTime>(
            name: "creation_date",
            schema: "initiatives",
            table: "resource_like",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTime>(
            name: "publication_date",
            schema: "initiatives",
            table: "resource",
            type: "timestamp without time zone",
            nullable: true,
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "creation_date",
            schema: "initiatives",
            table: "resource",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTime>(
            name: "reading_date",
            schema: "notifications",
            table: "notification",
            type: "timestamp without time zone",
            nullable: true,
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "creation_date",
            schema: "notifications",
            table: "notification",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTime>(
            name: "timestamp",
            schema: "logs",
            table: "logs",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTime>(
            name: "response_date",
            schema: "initiatives",
            table: "join_request",
            type: "timestamp without time zone",
            nullable: true,
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "creation_date",
            schema: "initiatives",
            table: "join_request",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTime>(
            name: "creation_date",
            schema: "initiatives",
            table: "join_invitation",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTime>(
            name: "creation_date",
            schema: "initiatives",
            table: "initiative_user",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<DateTime>(
            name: "creation_date",
            schema: "initiatives",
            table: "initiative",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTimeOffset),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");
    }
}
