using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planets.Migrations
{
    /// <inheritdoc />
    public partial class PlanetPropertiesAndValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanetProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanetProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanetPropertyValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    PlanetPropertyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanetPropertyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanetPropertyValues_PlanetProperties_PlanetPropertyId",
                        column: x => x.PlanetPropertyId,
                        principalTable: "PlanetProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanetPropertyValues_PlanetPropertyId",
                table: "PlanetPropertyValues",
                column: "PlanetPropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanetPropertyValues");

            migrationBuilder.DropTable(
                name: "PlanetProperties");
        }
    }
}
