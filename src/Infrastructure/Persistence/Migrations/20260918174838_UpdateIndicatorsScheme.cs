#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class UpdateIndicatorsScheme : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_indicator_value_indicator_group_indicator_group_id",
            schema: "indicators",
            table: "indicator_value");

        migrationBuilder.DropForeignKey(
            name: "FK_indicator_value_measure_unit_measure_unit_id",
            schema: "indicators",
            table: "indicator_value");

        migrationBuilder.DropForeignKey(
            name: "FK_map_legend_indicator_version_map_indicator_version_map_id",
            schema: "indicators",
            table: "map_legend");

        migrationBuilder.DropTable(
            name: "indicator_group",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_location",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_tag",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_version_map",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "measure_unit",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_version",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator",
            schema: "indicators");

        migrationBuilder.RenameColumn(
            name: "indicator_version_map_id",
            schema: "indicators",
            table: "map_legend",
            newName: "observation_version_map_id");

        migrationBuilder.RenameIndex(
            name: "IX_map_legend_indicator_version_map_id_title",
            schema: "indicators",
            table: "map_legend",
            newName: "IX_map_legend_observation_version_map_id_title");

        migrationBuilder.RenameColumn(
            name: "indicator_group_id",
            schema: "indicators",
            table: "indicator_value",
            newName: "observation_group_id");

        migrationBuilder.RenameColumn(
            name: "measure_unit_id",
            schema: "indicators",
            table: "indicator_value",
            newName: "indicator_type_id");

        migrationBuilder.RenameIndex(
            name: "IX_indicator_value_indicator_group_id",
            schema: "indicators",
            table: "indicator_value",
            newName: "IX_indicator_value_observation_group_id");

        migrationBuilder.RenameIndex(
            name: "IX_indicator_value_measure_unit_id",
            schema: "indicators",
            table: "indicator_value",
            newName: "IX_indicator_value_indicator_type_id");

        migrationBuilder.AlterColumn<string>(
            name: "name",
            schema: "indicators",
            table: "indicator_type",
            type: "character varying(70)",
            maxLength: 70,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(200)",
            oldMaxLength: 200);

        migrationBuilder.AddColumn<string>(
            name: "representation",
            schema: "indicators",
            table: "indicator_type",
            type: "character varying(10)",
            maxLength: 10,
            nullable: true);

        migrationBuilder.CreateTable(
            name: "indicator_topic",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator_topic", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "observation",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                indicator_topic_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_observation", x => x.id);
                table.ForeignKey(
                    name: "FK_observation_indicator_topic_indicator_topic_id",
                    column: x => x.indicator_topic_id,
                    principalSchema: "indicators",
                    principalTable: "indicator_topic",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_observation_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "observation_location",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                observation_id = table.Column<int>(type: "integer", nullable: false),
                location_id = table.Column<int>(type: "integer", nullable: false),
                locality = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_observation_location", x => x.id);
                table.ForeignKey(
                    name: "FK_observation_location_location_location_id",
                    column: x => x.location_id,
                    principalSchema: "geo",
                    principalTable: "location",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_observation_location_observation_observation_id",
                    column: x => x.observation_id,
                    principalSchema: "indicators",
                    principalTable: "observation",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "observation_tag",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                observation_id = table.Column<int>(type: "integer", nullable: false),
                tag_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_observation_tag", x => x.id);
                table.ForeignKey(
                    name: "FK_observation_tag_observation_observation_id",
                    column: x => x.observation_id,
                    principalSchema: "indicators",
                    principalTable: "observation",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_observation_tag_tag_tag_id",
                    column: x => x.tag_id,
                    principalSchema: "tags",
                    principalTable: "tag",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "observation_version",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                observation_id = table.Column<int>(type: "integer", nullable: false),
                creation_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                methodology = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                interpretation = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                considerations = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                authorship = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_observation_version", x => x.id);
                table.ForeignKey(
                    name: "FK_observation_version_observation_observation_id",
                    column: x => x.observation_id,
                    principalSchema: "indicators",
                    principalTable: "observation",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "observation_group",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                observation_version_id = table.Column<int>(type: "integer", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_observation_group", x => x.id);
                table.ForeignKey(
                    name: "FK_observation_group_category_category_id",
                    column: x => x.category_id,
                    principalSchema: "indicators",
                    principalTable: "category",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_observation_group_observation_version_observation_version_id",
                    column: x => x.observation_version_id,
                    principalSchema: "indicators",
                    principalTable: "observation_version",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "observation_version_map",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                observation_version_id = table.Column<int>(type: "integer", nullable: false),
                title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                image_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_observation_version_map", x => x.id);
                table.ForeignKey(
                    name: "FK_observation_version_map_observation_version_observation_ver~",
                    column: x => x.observation_version_id,
                    principalSchema: "indicators",
                    principalTable: "observation_version",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_indicator_topic_name",
            schema: "indicators",
            table: "indicator_topic",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_observation_indicator_topic_id",
            schema: "indicators",
            table: "observation",
            column: "indicator_topic_id");

        migrationBuilder.CreateIndex(
            name: "IX_observation_initiative_id_name",
            schema: "indicators",
            table: "observation",
            columns: new[] { "initiative_id", "name" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_observation_group_category_id",
            schema: "indicators",
            table: "observation_group",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "IX_observation_group_observation_version_id_category_id",
            schema: "indicators",
            table: "observation_group",
            columns: new[] { "observation_version_id", "category_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_observation_location_location_id",
            schema: "indicators",
            table: "observation_location",
            column: "location_id");

        migrationBuilder.CreateIndex(
            name: "IX_observation_location_observation_id_location_id_locality",
            schema: "indicators",
            table: "observation_location",
            columns: new[] { "observation_id", "location_id", "locality" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_observation_tag_observation_id_tag_id",
            schema: "indicators",
            table: "observation_tag",
            columns: new[] { "observation_id", "tag_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_observation_tag_tag_id",
            schema: "indicators",
            table: "observation_tag",
            column: "tag_id");

        migrationBuilder.CreateIndex(
            name: "IX_observation_version_observation_id_version",
            schema: "indicators",
            table: "observation_version",
            columns: new[] { "observation_id", "version" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_observation_version_map_observation_version_id_title",
            schema: "indicators",
            table: "observation_version_map",
            columns: new[] { "observation_version_id", "title" },
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_indicator_value_indicator_type_indicator_type_id",
            schema: "indicators",
            table: "indicator_value",
            column: "indicator_type_id",
            principalSchema: "indicators",
            principalTable: "indicator_type",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_indicator_value_observation_group_observation_group_id",
            schema: "indicators",
            table: "indicator_value",
            column: "observation_group_id",
            principalSchema: "indicators",
            principalTable: "observation_group",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_map_legend_observation_version_map_observation_version_map_~",
            schema: "indicators",
            table: "map_legend",
            column: "observation_version_map_id",
            principalSchema: "indicators",
            principalTable: "observation_version_map",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_indicator_value_indicator_type_indicator_type_id",
            schema: "indicators",
            table: "indicator_value");

        migrationBuilder.DropForeignKey(
            name: "FK_indicator_value_observation_group_observation_group_id",
            schema: "indicators",
            table: "indicator_value");

        migrationBuilder.DropForeignKey(
            name: "FK_map_legend_observation_version_map_observation_version_map_~",
            schema: "indicators",
            table: "map_legend");

        migrationBuilder.DropTable(
            name: "observation_group",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "observation_location",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "observation_tag",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "observation_version_map",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "observation_version",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "observation",
            schema: "indicators");

        migrationBuilder.DropTable(
            name: "indicator_topic",
            schema: "indicators");

        migrationBuilder.DropColumn(
            name: "representation",
            schema: "indicators",
            table: "indicator_type");

        migrationBuilder.RenameColumn(
            name: "observation_version_map_id",
            schema: "indicators",
            table: "map_legend",
            newName: "indicator_version_map_id");

        migrationBuilder.RenameIndex(
            name: "IX_map_legend_observation_version_map_id_title",
            schema: "indicators",
            table: "map_legend",
            newName: "IX_map_legend_indicator_version_map_id_title");

        migrationBuilder.RenameColumn(
            name: "observation_group_id",
            schema: "indicators",
            table: "indicator_value",
            newName: "indicator_group_id");

        migrationBuilder.RenameColumn(
            name: "indicator_type_id",
            schema: "indicators",
            table: "indicator_value",
            newName: "measure_unit_id");

        migrationBuilder.RenameIndex(
            name: "IX_indicator_value_observation_group_id",
            schema: "indicators",
            table: "indicator_value",
            newName: "IX_indicator_value_indicator_group_id");

        migrationBuilder.RenameIndex(
            name: "IX_indicator_value_indicator_type_id",
            schema: "indicators",
            table: "indicator_value",
            newName: "IX_indicator_value_measure_unit_id");

        migrationBuilder.AlterColumn<string>(
            name: "name",
            schema: "indicators",
            table: "indicator_type",
            type: "character varying(200)",
            maxLength: 200,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(70)",
            oldMaxLength: 70);

        migrationBuilder.CreateTable(
            name: "indicator",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                indicator_type_id = table.Column<int>(type: "integer", nullable: false),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
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
                    principalSchema: "tags",
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
                authorship = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                considerations = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                creation_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                interpretation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                methodology = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
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

        migrationBuilder.CreateTable(
            name: "indicator_group",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                category_id = table.Column<int>(type: "integer", nullable: false),
                indicator_version_id = table.Column<int>(type: "integer", nullable: false),
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
            name: "indicator_version_map",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                indicator_version_id = table.Column<int>(type: "integer", nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                image_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
            name: "IX_indicator_version_indicator_id_version",
            schema: "indicators",
            table: "indicator_version",
            columns: new[] { "indicator_id", "version" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_indicator_version_map_indicator_version_id_title",
            schema: "indicators",
            table: "indicator_version_map",
            columns: new[] { "indicator_version_id", "title" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_measure_unit_name",
            schema: "indicators",
            table: "measure_unit",
            column: "name",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_indicator_value_indicator_group_indicator_group_id",
            schema: "indicators",
            table: "indicator_value",
            column: "indicator_group_id",
            principalSchema: "indicators",
            principalTable: "indicator_group",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_indicator_value_measure_unit_measure_unit_id",
            schema: "indicators",
            table: "indicator_value",
            column: "measure_unit_id",
            principalSchema: "indicators",
            principalTable: "measure_unit",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_map_legend_indicator_version_map_indicator_version_map_id",
            schema: "indicators",
            table: "map_legend",
            column: "indicator_version_map_id",
            principalSchema: "indicators",
            principalTable: "indicator_version_map",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }
}
