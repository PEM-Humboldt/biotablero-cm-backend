#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System.IO;
using System.Reflection;

using Microsoft.EntityFrameworkCore.Migrations;

using NetTopologySuite.Geometries;

/// <inheritdoc />
public partial class AddInitiativeTrigger : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "main_location_id",
            schema: "initiatives",
            table: "initiative",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Point>(
            name: "coordinate",
            schema: "initiatives",
            table: "initiative",
            type: "geometry(Point, 4326)",
            nullable: false,
            defaultValueSql: "ST_GeomFromText('POINT EMPTY', 4326)",
            oldClrType: typeof(Point),
            oldType: "geometry(Point, 4326)");

        // Add Initiative triggers
        var resourceName = "IAVH.BioTablero.CM.Infrastructure.Persistence.Scripts.fn_update_initiative_computed_fields.sql";

        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"The embedded script could not be loaded: {resourceName}");

        using var reader = new StreamReader(stream);
        string sqlScript = reader.ReadToEnd();

        migrationBuilder.Sql(sqlScript);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "main_location_id",
            schema: "initiatives",
            table: "initiative",
            type: "integer",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer",
            oldDefaultValue: 0);

        migrationBuilder.AlterColumn<Point>(
            name: "coordinate",
            schema: "initiatives",
            table: "initiative",
            type: "geometry(Point, 4326)",
            nullable: false,
            oldClrType: typeof(Point),
            oldType: "geometry(Point, 4326)",
            oldDefaultValueSql: "ST_GeomFromText('POINT EMPTY', 4326)");

        migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_initiative_compute_fields ON initiatives.initiative;
            DROP TRIGGER IF EXISTS trg_initiative_location_update ON initiatives.initiative_location;
            DROP FUNCTION IF EXISTS initiatives.fn_update_initiative_computed_fields();
        ");
    }
}
