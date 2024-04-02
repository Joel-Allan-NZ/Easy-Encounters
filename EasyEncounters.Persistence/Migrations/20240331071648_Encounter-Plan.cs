using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEncounters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EncounterPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Plan",
                table: "Encounters",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plan",
                table: "Encounters");
        }
    }
}
