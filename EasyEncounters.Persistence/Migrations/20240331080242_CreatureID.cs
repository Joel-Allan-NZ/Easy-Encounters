using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEncounters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreatureID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatureID",
                table: "ActiveEncounterCreatures",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatureID",
                table: "ActiveEncounterCreatures");
        }
    }
}
