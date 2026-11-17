#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

using NetTopologySuite.Geometries;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddPolygonsAndTags : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_initiative_user_initiative_user_level_level_id",
            schema: "initiatives",
            table: "initiative_user");

        migrationBuilder.RenameColumn(
            name: "level_id",
            schema: "initiatives",
            table: "initiative_user",
            newName: "initiative_user_level_id");

        migrationBuilder.RenameIndex(
            name: "IX_initiative_user_level_id",
            schema: "initiatives",
            table: "initiative_user",
            newName: "IX_initiative_user_initiative_user_level_id");

        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:postgis", ",,");

        migrationBuilder.AddColumn<Point>(
            name: "coordinate",
            schema: "initiatives",
            table: "initiative",
            type: "geometry(Point, 4326)",
            nullable: false);

        migrationBuilder.AddColumn<Geometry>(
            name: "polygon",
            schema: "initiatives",
            table: "initiative",
            type: "geometry(Polygon, 4326)",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "location_polygon",
            schema: "geo",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                geometry = table.Column<Geometry>(type: "geometry(MultiPolygon, 4326)", nullable: false),
                geometry_simplified = table.Column<string>(type: "text", nullable: false),
                location_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_location_polygon", x => x.id);
                table.ForeignKey(
                    name: "FK_location_polygon_location_location_id",
                    column: x => x.location_id,
                    principalSchema: "geo",
                    principalTable: "location",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "tag_category",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tag_category", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "tag",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                url = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                tag_category_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tag", x => x.id);
                table.ForeignKey(
                    name: "FK_tag_tag_category_tag_category_id",
                    column: x => x.tag_category_id,
                    principalSchema: "initiatives",
                    principalTable: "tag_category",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "initiative_tag",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                tag_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_tag", x => x.id);
                table.ForeignKey(
                    name: "FK_initiative_tag_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_initiative_tag_tag_tag_id",
                    column: x => x.tag_id,
                    principalSchema: "initiatives",
                    principalTable: "tag",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            schema: "initiatives",
            table: "tag_category",
            columns: new[] { "id", "name" },
            values: new object[,]
            {
                { 1, "PoliticalContext" },
                { 2, "SocialContext" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_initiative_tag_initiative_id",
            schema: "initiatives",
            table: "initiative_tag",
            column: "initiative_id");

        migrationBuilder.CreateIndex(
            name: "IX_initiative_tag_tag_id",
            schema: "initiatives",
            table: "initiative_tag",
            column: "tag_id");

        migrationBuilder.CreateIndex(
            name: "IX_location_polygon_location_id",
            schema: "geo",
            table: "location_polygon",
            column: "location_id",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_tag_name",
            schema: "initiatives",
            table: "tag",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_tag_tag_category_id",
            schema: "initiatives",
            table: "tag",
            column: "tag_category_id");

        migrationBuilder.CreateIndex(
            name: "IX_tag_category_name",
            schema: "initiatives",
            table: "tag_category",
            column: "name",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_initiative_user_initiative_user_level_initiative_user_level~",
            schema: "initiatives",
            table: "initiative_user",
            column: "initiative_user_level_id",
            principalSchema: "initiatives",
            principalTable: "initiative_user_level",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_initiative_user_initiative_user_level_initiative_user_level~",
            schema: "initiatives",
            table: "initiative_user");

        migrationBuilder.DropTable(
            name: "initiative_tag",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "location_polygon",
            schema: "geo");

        migrationBuilder.DropTable(
            name: "tag",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "tag_category",
            schema: "initiatives");

        migrationBuilder.DropColumn(
            name: "coordinate",
            schema: "initiatives",
            table: "initiative");

        migrationBuilder.DropColumn(
            name: "polygon",
            schema: "initiatives",
            table: "initiative");

        migrationBuilder.RenameColumn(
            name: "initiative_user_level_id",
            schema: "initiatives",
            table: "initiative_user",
            newName: "level_id");

        migrationBuilder.RenameIndex(
            name: "IX_initiative_user_initiative_user_level_id",
            schema: "initiatives",
            table: "initiative_user",
            newName: "IX_initiative_user_level_id");

        migrationBuilder.AlterDatabase()
            .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

        migrationBuilder.AddForeignKey(
            name: "FK_initiative_user_initiative_user_level_level_id",
            schema: "initiatives",
            table: "initiative_user",
            column: "level_id",
            principalSchema: "initiatives",
            principalTable: "initiative_user_level",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }
}
