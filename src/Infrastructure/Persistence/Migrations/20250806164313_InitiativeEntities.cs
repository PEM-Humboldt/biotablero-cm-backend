#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class InitiativeEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "initiatives");

        migrationBuilder.EnsureSchema(
            name: "geo");

        migrationBuilder.CreateTable(
            name: "initiative",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                logo_url = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "initiative_user_level",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_user_level", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "location",
            schema: "geo",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                parent_id = table.Column<int>(type: "integer", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_location", x => x.id);
                table.ForeignKey(
                    name: "FK_location_location_parent_id",
                    column: x => x.parent_id,
                    principalSchema: "geo",
                    principalTable: "location",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "initiative_contact",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                phone = table.Column<string>(type: "text", nullable: true),
                email = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_contact", x => x.id);
                table.ForeignKey(
                    name: "FK_initiative_contact_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "initiative_user",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                user_name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                level_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_user", x => x.id);
                table.ForeignKey(
                    name: "FK_initiative_user_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_initiative_user_initiative_user_level_level_id",
                    column: x => x.level_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative_user_level",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "initiative_location",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                location_id = table.Column<int>(type: "integer", nullable: false),
                locality = table.Column<string>(type: "text", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_location", x => x.id);
                table.ForeignKey(
                    name: "FK_initiative_location_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_initiative_location_location_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "geo",
                    principalTable: "location",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            schema: "initiatives",
            table: "initiative_user_level",
            columns: new[] { "id", "name" },
            values: new object[,]
            {
                { 1, "Leader" },
                { 2, "Member" },
                { 3, "Reader" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_initiative_contact_initiative_id",
            schema: "initiatives",
            table: "initiative_contact",
            column: "initiative_id");

        migrationBuilder.CreateIndex(
            name: "IX_initiative_location_initiative_id_location_id",
            schema: "initiatives",
            table: "initiative_location",
            columns: new[] { "initiative_id", "location_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_initiative_user_initiative_id",
            schema: "initiatives",
            table: "initiative_user",
            column: "initiative_id");

        migrationBuilder.CreateIndex(
            name: "IX_initiative_user_level_id",
            schema: "initiatives",
            table: "initiative_user",
            column: "level_id");

        migrationBuilder.CreateIndex(
            name: "IX_initiative_user_user_name_initiative_id",
            schema: "initiatives",
            table: "initiative_user",
            columns: new[] { "user_name", "initiative_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_initiative_user_level_name",
            schema: "initiatives",
            table: "initiative_user_level",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_location_code",
            schema: "geo",
            table: "location",
            column: "code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_location_parent_id",
            schema: "geo",
            table: "location",
            column: "parent_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "initiative_contact",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "initiative_location",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "initiative_user",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "location",
            schema: "geo");

        migrationBuilder.DropTable(
            name: "initiative",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "initiative_user_level",
            schema: "initiatives");
    }
}
