using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEncounters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ActiveEncounterParty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartyId",
                table: "ActiveEncounters",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ActiveEncounters_PartyId",
                table: "ActiveEncounters",
                column: "PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveEncounters_Parties_PartyId",
                table: "ActiveEncounters",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveEncounters_Parties_PartyId",
                table: "ActiveEncounters");

            migrationBuilder.DropIndex(
                name: "IX_ActiveEncounters_PartyId",
                table: "ActiveEncounters");

            migrationBuilder.DropColumn(
                name: "PartyId",
                table: "ActiveEncounters");
        }
    }
}
