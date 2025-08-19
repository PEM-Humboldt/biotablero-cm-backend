#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

using NetTopologySuite.Geometries;

/// <inheritdoc />
public partial class AddLocationPolygons : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:postgis", ",,");

        migrationBuilder.CreateTable(
            name: "location_polygon",
            schema: "geo",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                geometry = table.Column<Geometry>(type: "geometry(MultiPolygon, 4326)", nullable: false),
                geometry_simplified = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_location_polygon", x => x.id);
                table.ForeignKey(
                    name: "FK_location_polygon_location_id",
                    column: x => x.id,
                    principalSchema: "geo",
                    principalTable: "location",
                    principalColumn: "id");
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "location_polygon",
            schema: "geo");

        migrationBuilder.AlterDatabase()
            .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");
    }
}
