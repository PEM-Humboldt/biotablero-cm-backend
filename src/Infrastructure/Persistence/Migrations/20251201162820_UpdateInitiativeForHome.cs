#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class UpdateInitiativeForHome : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "creation_date",
            schema: "initiatives",
            table: "initiative_user",
            type: "timestamp without time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AddColumn<string>(
            name: "focus_area",
            schema: "initiatives",
            table: "initiative_user",
            type: "character varying(200)",
            maxLength: 200,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "influence_area",
            schema: "initiatives",
            table: "initiative",
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "objective",
            schema: "initiatives",
            table: "initiative",
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "short_name",
            schema: "initiatives",
            table: "initiative",
            type: "character varying(120)",
            maxLength: 120,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "creation_date",
            schema: "initiatives",
            table: "initiative_user");

        migrationBuilder.DropColumn(
            name: "focus_area",
            schema: "initiatives",
            table: "initiative_user");

        migrationBuilder.DropColumn(
            name: "influence_area",
            schema: "initiatives",
            table: "initiative");

        migrationBuilder.DropColumn(
            name: "objective",
            schema: "initiatives",
            table: "initiative");

        migrationBuilder.DropColumn(
            name: "short_name",
            schema: "initiatives",
            table: "initiative");
    }
}
