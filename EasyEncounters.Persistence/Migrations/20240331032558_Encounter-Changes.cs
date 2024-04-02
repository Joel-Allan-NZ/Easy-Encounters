using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEncounters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EncounterChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveEncounterCreatures_Encounters_ActiveEncounterId",
                table: "ActiveEncounterCreatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveEncounterCreatures_Encounters_ActiveEncounterId1",
                table: "ActiveEncounterCreatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_ActiveEncounterCreatures_ActiveTurnId",
                table: "Encounters");

            migrationBuilder.DropIndex(
                name: "IX_Encounters_ActiveTurnId",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "ActiveTurnId",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "Log",
                table: "Encounters");

            migrationBuilder.CreateTable(
                name: "ActiveEncounters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveTurnId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Log = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveEncounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveEncounters_ActiveEncounterCreatures_ActiveTurnId",
                        column: x => x.ActiveTurnId,
                        principalTable: "ActiveEncounterCreatures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EncounterCreatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatureId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    EncounterId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncounterCreatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncounterCreatures_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EncounterCreatures_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveEncounters_ActiveTurnId",
                table: "ActiveEncounters",
                column: "ActiveTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_EncounterCreatures_CreatureId",
                table: "EncounterCreatures",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_EncounterCreatures_EncounterId",
                table: "EncounterCreatures",
                column: "EncounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveEncounterCreatures_ActiveEncounters_ActiveEncounterId",
                table: "ActiveEncounterCreatures",
                column: "ActiveEncounterId",
                principalTable: "ActiveEncounters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveEncounterCreatures_ActiveEncounters_ActiveEncounterId1",
                table: "ActiveEncounterCreatures",
                column: "ActiveEncounterId1",
                principalTable: "ActiveEncounters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveEncounterCreatures_ActiveEncounters_ActiveEncounterId",
                table: "ActiveEncounterCreatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveEncounterCreatures_ActiveEncounters_ActiveEncounterId1",
                table: "ActiveEncounterCreatures");

            migrationBuilder.DropTable(
                name: "ActiveEncounters");

            migrationBuilder.DropTable(
                name: "EncounterCreatures");

            migrationBuilder.AddColumn<Guid>(
                name: "ActiveTurnId",
                table: "Encounters",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Encounters",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Log",
                table: "Encounters",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_ActiveTurnId",
                table: "Encounters",
                column: "ActiveTurnId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveEncounterCreatures_Encounters_ActiveEncounterId",
                table: "ActiveEncounterCreatures",
                column: "ActiveEncounterId",
                principalTable: "Encounters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveEncounterCreatures_Encounters_ActiveEncounterId1",
                table: "ActiveEncounterCreatures",
                column: "ActiveEncounterId1",
                principalTable: "Encounters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_ActiveEncounterCreatures_ActiveTurnId",
                table: "Encounters",
                column: "ActiveTurnId",
                principalTable: "ActiveEncounterCreatures",
                principalColumn: "Id");
        }
    }
}
