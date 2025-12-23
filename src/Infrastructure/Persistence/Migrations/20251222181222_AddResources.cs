#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddResources : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "resource_type",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_resource_type", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "resource",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                resource_type_id = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                publication_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                is_draft = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_resource", x => x.id);
                table.ForeignKey(
                    name: "FK_resource_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_resource_resource_type_resource_type_id",
                    column: x => x.resource_type_id,
                    principalSchema: "initiatives",
                    principalTable: "resource_type",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "resource_file",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                resource_id = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_resource_file", x => x.id);
                table.ForeignKey(
                    name: "FK_resource_file_resource_resource_id",
                    column: x => x.resource_id,
                    principalSchema: "initiatives",
                    principalTable: "resource",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "resource_like",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                resource_id = table.Column<int>(type: "integer", nullable: false),
                user_name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_resource_like", x => x.id);
                table.ForeignKey(
                    name: "FK_resource_like_resource_resource_id",
                    column: x => x.resource_id,
                    principalSchema: "initiatives",
                    principalTable: "resource",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "resource_link",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                resource_id = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_resource_link", x => x.id);
                table.ForeignKey(
                    name: "FK_resource_link_resource_resource_id",
                    column: x => x.resource_id,
                    principalSchema: "initiatives",
                    principalTable: "resource",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "resource_tag",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                resource_id = table.Column<int>(type: "integer", nullable: false),
                tag_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_resource_tag", x => x.id);
                table.ForeignKey(
                    name: "FK_resource_tag_resource_resource_id",
                    column: x => x.resource_id,
                    principalSchema: "initiatives",
                    principalTable: "resource",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_resource_tag_tag_tag_id",
                    column: x => x.tag_id,
                    principalSchema: "initiatives",
                    principalTable: "tag",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_resource_initiative_id",
            schema: "initiatives",
            table: "resource",
            column: "initiative_id");

        migrationBuilder.CreateIndex(
            name: "IX_resource_name",
            schema: "initiatives",
            table: "resource",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_resource_resource_type_id",
            schema: "initiatives",
            table: "resource",
            column: "resource_type_id");

        migrationBuilder.CreateIndex(
            name: "IX_resource_file_resource_id_url",
            schema: "initiatives",
            table: "resource_file",
            columns: new[] { "resource_id", "url" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_resource_like_resource_id_user_name",
            schema: "initiatives",
            table: "resource_like",
            columns: new[] { "resource_id", "user_name" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_resource_link_resource_id_url",
            schema: "initiatives",
            table: "resource_link",
            columns: new[] { "resource_id", "url" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_resource_tag_resource_id_tag_id",
            schema: "initiatives",
            table: "resource_tag",
            columns: new[] { "resource_id", "tag_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_resource_tag_tag_id",
            schema: "initiatives",
            table: "resource_tag",
            column: "tag_id");

        migrationBuilder.CreateIndex(
            name: "IX_resource_type_name",
            schema: "initiatives",
            table: "resource_type",
            column: "name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "resource_file",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "resource_like",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "resource_link",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "resource_tag",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "resource",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "resource_type",
            schema: "initiatives");
    }
}
