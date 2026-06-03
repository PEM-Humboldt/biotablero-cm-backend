#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddIndicators : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "category",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                parent_id = table.Column<int>(type: "integer", nullable: true),
                name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                description = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_category", x => x.id);
                table.ForeignKey(
                    name: "FK_category_category_parent_id",
                    column: x => x.parent_id,
                    principalSchema: "indicators",
                    principalTable: "category",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "indicator_version_map",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                indicator_version_id = table.Column<int>(type: "integer", nullable: false),
                title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                image_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator_version_map", x => x.id);
                table.ForeignKey(
                    name: "FK_indicator_version_map_indicator_version_indicator_version_id",
                    column: x => x.indicator_version_id,
                    principalSchema: "indicators",
                    principalTable: "indicator_version",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "measure_unit",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                representation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_measure_unit", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "indicator_group",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                indicator_version_id = table.Column<int>(type: "integer", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator_group", x => x.id);
                table.ForeignKey(
                    name: "FK_indicator_group_category_category_id",
                    column: x => x.category_id,
                    principalSchema: "indicators",
                    principalTable: "category",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_indicator_group_indicator_version_indicator_version_id",
                    column: x => x.indicator_version_id,
                    principalSchema: "indicators",
                    principalTable: "indicator_version",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "map_legend",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                indicator_version_map_id = table.Column<int>(type: "integer", nullable: false),
                title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_map_legend", x => x.id);
                table.ForeignKey(
                    name: "FK_map_legend_indicator_version_map_indicator_version_map_id",
                    column: x => x.indicator_version_map_id,
                    principalSchema: "indicators",
                    principalTable: "indicator_version_map",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "indicator_value",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                indicator_group_id = table.Column<int>(type: "integer", nullable: false),
                measure_unit_id = table.Column<int>(type: "integer", nullable: false),
                date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                date_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                value = table.Column<float>(type: "real", nullable: false),
                upper_limit = table.Column<float>(type: "real", nullable: true),
                lower_limit = table.Column<float>(type: "real", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator_value", x => x.id);
                table.ForeignKey(
                    name: "FK_indicator_value_indicator_group_indicator_group_id",
                    column: x => x.indicator_group_id,
                    principalSchema: "indicators",
                    principalTable: "indicator_group",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_indicator_value_measure_unit_measure_unit_id",
                    column: x => x.measure_unit_id,
                    principalSchema: "indicators",
                    principalTable: "measure_unit",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "map_legend_item",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                map_legend_id = table.Column<int>(type: "integer", nullable: false),
                color_code = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                value = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_map_legend_item", x => x.id);
                table.ForeignKey(
                    name: "FK_map_legend_item_map_legend_map_legend_id",
                    column: x => x.map_legend_id,
                    principalSchema: "indicators",
                    principalTable: "map_legend",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_category_parent_id_name",
            schema: "indicators",
            table: "category",
            columns: new[] { "parent_id", "name" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_indicator_group_category_id",
            schema: "indicators",
            table: "indicator_group",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "IX_indicator_group_indicator_version_id_category_id",
            schema: "indicators",
            table: "indicator_group",
            columns: new[] { "indicator_version_id", "category_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_indicator_value_indicator_group_id",
            schema: "indicators",
            table: "indicator_value",
            column: "indicator_group_id");

        migrationBuilder.CreateIndex(
            name: "IX_indicator_value_measure_unit_id",
            schema: "indicators",
            table: "indicator_value",
            column: "measure_unit_id");

        migrationBuilder.CreateIndex(
            name: "IX_indicator_version_map_indicator_version_id_title",
            schema: "indicators",
            table: "indicator_version_map",
            columns: new[] { "indicator_version_id", "title" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_map_legend_indicator_version_map_id_title",
            schema: "indicators",
            table: "map_legend",
            columns: new[] { "indicator_version_map_id", "title" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_map_legend_item_map_legend_id_color_code",
            schema: "indicators",
            table: "map_legend_item",
            columns: new[] { "map_legend_id", "color_code" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_measure_unit_name",
            schema: "indicators",
            table: "measure_unit",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_measure_unit_representation",
            schema: "indicators",
            table: "measure_unit",
            column: "representation",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "indicator_value",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "map_legend_item",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_group",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "measure_unit",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "map_legend",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "category",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_version_map",
            schema: "indicators");
    }
}
