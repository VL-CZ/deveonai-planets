using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planets.Migrations
{
    /// <inheritdoc />
    public partial class PropertiesOfPlanets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanetPlanetPropertyValue",
                columns: table => new
                {
                    PlanetsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyValuesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanetPlanetPropertyValue", x => new { x.PlanetsId, x.PropertyValuesId });
                    table.ForeignKey(
                        name: "FK_PlanetPlanetPropertyValue_PlanetPropertyValues_PropertyValu~",
                        column: x => x.PropertyValuesId,
                        principalTable: "PlanetPropertyValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanetPlanetPropertyValue_Planets_PlanetsId",
                        column: x => x.PlanetsId,
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanetPlanetPropertyValue_PropertyValuesId",
                table: "PlanetPlanetPropertyValue",
                column: "PropertyValuesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanetPlanetPropertyValue");
        }
    }
}
