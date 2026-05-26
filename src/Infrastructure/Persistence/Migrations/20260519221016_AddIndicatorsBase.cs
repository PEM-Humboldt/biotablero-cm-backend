#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddIndicatorsBase : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "indicators");

        migrationBuilder.AlterColumn<string>(
            name: "locality",
            schema: "initiatives",
            table: "initiative_location",
            type: "character varying(300)",
            maxLength: 300,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.CreateTable(
            name: "indicator_type",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator_type", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "indicator",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                indicator_type_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator", x => x.id);
                table.ForeignKey(
                    name: "FK_indicator_indicator_type_indicator_type_id",
                    column: x => x.indicator_type_id,
                    principalSchema: "indicators",
                    principalTable: "indicator_type",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_indicator_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "indicator_location",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                indicator_id = table.Column<int>(type: "integer", nullable: false),
                location_id = table.Column<int>(type: "integer", nullable: false),
                locality = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator_location", x => x.id);
                table.ForeignKey(
                    name: "FK_indicator_location_indicator_indicator_id",
                    column: x => x.indicator_id,
                    principalSchema: "indicators",
                    principalTable: "indicator",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_indicator_location_location_location_id",
                    column: x => x.location_id,
                    principalSchema: "geo",
                    principalTable: "location",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "indicator_tag",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                indicator_id = table.Column<int>(type: "integer", nullable: false),
                tag_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator_tag", x => x.id);
                table.ForeignKey(
                    name: "FK_indicator_tag_indicator_indicator_id",
                    column: x => x.indicator_id,
                    principalSchema: "indicators",
                    principalTable: "indicator",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_indicator_tag_tag_tag_id",
                    column: x => x.tag_id,
                    principalSchema: "initiatives",
                    principalTable: "tag",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "indicator_version",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                indicator_id = table.Column<int>(type: "integer", nullable: false),
                creation_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                methodology = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                interpretation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                considerations = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                authorship = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator_version", x => x.id);
                table.ForeignKey(
                    name: "FK_indicator_version_indicator_indicator_id",
                    column: x => x.indicator_id,
                    principalSchema: "indicators",
                    principalTable: "indicator",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_indicator_indicator_type_id",
            schema: "indicators",
            table: "indicator",
            column: "indicator_type_id");

        migrationBuilder.CreateIndex(
            name: "IX_indicator_initiative_id",
            schema: "indicators",
            table: "indicator",
            column: "initiative_id");

        migrationBuilder.CreateIndex(
            name: "IX_indicator_location_indicator_id_location_id_locality",
            schema: "indicators",
            table: "indicator_location",
            columns: new[] { "indicator_id", "location_id", "locality" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_indicator_location_location_id",
            schema: "indicators",
            table: "indicator_location",
            column: "location_id");

        migrationBuilder.CreateIndex(
            name: "IX_indicator_tag_indicator_id_tag_id",
            schema: "indicators",
            table: "indicator_tag",
            columns: new[] { "indicator_id", "tag_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_indicator_tag_tag_id",
            schema: "indicators",
            table: "indicator_tag",
            column: "tag_id");

        migrationBuilder.CreateIndex(
            name: "IX_indicator_type_name",
            schema: "indicators",
            table: "indicator_type",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_indicator_version_indicator_id_version",
            schema: "indicators",
            table: "indicator_version",
            columns: new[] { "indicator_id", "version" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "indicator_location",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_tag",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_version",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_type",
            schema: "indicators");

        migrationBuilder.AlterColumn<string>(
            name: "locality",
            schema: "initiatives",
            table: "initiative_location",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "character varying(300)",
            oldMaxLength: 300,
            oldNullable: true);
    }
}
