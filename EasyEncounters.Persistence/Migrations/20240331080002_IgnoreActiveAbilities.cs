using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEncounters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IgnoreActiveAbilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abilities_ActiveEncounterCreatures_ActiveEncounterCreatureId1",
                table: "Abilities");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_ActiveEncounterCreatureId1",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "ActiveEncounterCreatureId1",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "ResolutionValue",
                table: "Abilities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActiveEncounterCreatureId1",
                table: "Abilities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Abilities",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ResolutionValue",
                table: "Abilities",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_ActiveEncounterCreatureId1",
                table: "Abilities",
                column: "ActiveEncounterCreatureId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_ActiveEncounterCreatures_ActiveEncounterCreatureId1",
                table: "Abilities",
                column: "ActiveEncounterCreatureId1",
                principalTable: "ActiveEncounterCreatures",
                principalColumn: "Id");
        }
    }
}
