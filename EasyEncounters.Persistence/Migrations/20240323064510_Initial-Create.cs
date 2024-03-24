using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEncounters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Creatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AC = table.Column<int>(type: "INTEGER", nullable: false),
                    Alignment = table.Column<int>(type: "INTEGER", nullable: false),
                    AttackDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Charisma = table.Column<int>(type: "INTEGER", nullable: false),
                    CharismaSave = table.Column<int>(type: "INTEGER", nullable: false),
                    ConditionImmunities = table.Column<int>(type: "INTEGER", nullable: false),
                    Constitution = table.Column<int>(type: "INTEGER", nullable: false),
                    ConstitutionSave = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatureSubtype = table.Column<string>(type: "TEXT", nullable: true),
                    CreatureType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Dexterity = table.Column<int>(type: "INTEGER", nullable: false),
                    DexteritySave = table.Column<int>(type: "INTEGER", nullable: false),
                    DMControl = table.Column<bool>(type: "INTEGER", nullable: false),
                    Features = table.Column<string>(type: "TEXT", nullable: true),
                    Hyperlink = table.Column<string>(type: "TEXT", nullable: true),
                    Immunity = table.Column<int>(type: "INTEGER", nullable: false),
                    InitiativeAdvantage = table.Column<bool>(type: "INTEGER", nullable: false),
                    InitiativeBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    Intelligence = table.Column<int>(type: "INTEGER", nullable: false),
                    IntelligenceSave = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelOrCR = table.Column<double>(type: "REAL", nullable: false),
                    MaxHP = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxHPString = table.Column<string>(type: "TEXT", nullable: true),
                    MaxLegendaryActions = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxLegendaryResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    Movement = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ProficiencyBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    Resistance = table.Column<int>(type: "INTEGER", nullable: false),
                    Senses = table.Column<string>(type: "TEXT", nullable: true),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    SpellSlots = table.Column<string>(type: "TEXT", nullable: true),
                    SpellStat = table.Column<int>(type: "INTEGER", nullable: false),
                    Strength = table.Column<int>(type: "INTEGER", nullable: false),
                    StrengthSave = table.Column<int>(type: "INTEGER", nullable: false),
                    Vulnerability = table.Column<int>(type: "INTEGER", nullable: false),
                    Wisdom = table.Column<int>(type: "INTEGER", nullable: false),
                    WisdomSave = table.Column<int>(type: "INTEGER", nullable: false),
                    XP = table.Column<int>(type: "INTEGER", nullable: false),
                    NotProficient = table.Column<int>(type: "INTEGER", nullable: false),
                    HalfProficient = table.Column<int>(type: "INTEGER", nullable: false),
                    Proficient = table.Column<int>(type: "INTEGER", nullable: false),
                    Expertise = table.Column<int>(type: "INTEGER", nullable: false),
                    ToolSkills = table.Column<string>(type: "TEXT", nullable: true),
                    Languages = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CampaignId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PartyXPThresholds = table.Column<string>(type: "TEXT", nullable: false),
                    PartyDescription = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parties_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CreatureParty",
                columns: table => new
                {
                    MembersId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PartyId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureParty", x => new { x.MembersId, x.PartyId });
                    table.ForeignKey(
                        name: "FK_CreatureParty_Creatures_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureParty_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActionSpeed = table.Column<int>(type: "INTEGER", nullable: false),
                    CastingComponents = table.Column<int>(type: "INTEGER", nullable: false),
                    CastTimeString = table.Column<string>(type: "TEXT", nullable: true),
                    Concentration = table.Column<bool>(type: "INTEGER", nullable: false),
                    DamageTypes = table.Column<int>(type: "INTEGER", nullable: false),
                    EffectDescription = table.Column<string>(type: "TEXT", nullable: true),
                    MagicSchool = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialCost = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Resolution = table.Column<int>(type: "INTEGER", nullable: false),
                    ResolutionStat = table.Column<int>(type: "INTEGER", nullable: false),
                    SaveType = table.Column<int>(type: "INTEGER", nullable: false),
                    SpellLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetAreaSize = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetAreaType = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetDistance = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetDistanceType = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeDurationType = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveEncounterCreatureId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    ResolutionValue = table.Column<int>(type: "INTEGER", nullable: true),
                    ActiveEncounterCreatureId1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbilityCreature",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatureId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityCreature", x => new { x.AbilitiesId, x.CreatureId });
                    table.ForeignKey(
                        name: "FK_AbilityCreature_Abilities_AbilitiesId",
                        column: x => x.AbilitiesId,
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilityCreature_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActiveEncounterCreatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActiveSpellSlots = table.Column<string>(type: "TEXT", nullable: true),
                    ActiveConditions = table.Column<int>(type: "INTEGER", nullable: false),
                    Concentrating = table.Column<bool>(type: "INTEGER", nullable: false),
                    CurrentHP = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentLegendaryActions = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentLegendaryResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    Dead = table.Column<bool>(type: "INTEGER", nullable: false),
                    EncounterID = table.Column<Guid>(type: "TEXT", nullable: false),
                    EncounterMaxHP = table.Column<int>(type: "INTEGER", nullable: false),
                    EncounterName = table.Column<string>(type: "TEXT", nullable: true),
                    Initiative = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    Reaction = table.Column<bool>(type: "INTEGER", nullable: false),
                    TempHP = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveEncounterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ActiveEncounterId1 = table.Column<Guid>(type: "TEXT", nullable: true),
                    AC = table.Column<int>(type: "INTEGER", nullable: false),
                    Alignment = table.Column<int>(type: "INTEGER", nullable: false),
                    AttackDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Charisma = table.Column<int>(type: "INTEGER", nullable: false),
                    CharismaSave = table.Column<int>(type: "INTEGER", nullable: false),
                    ConditionImmunities = table.Column<int>(type: "INTEGER", nullable: false),
                    Constitution = table.Column<int>(type: "INTEGER", nullable: false),
                    ConstitutionSave = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatureSubtype = table.Column<string>(type: "TEXT", nullable: true),
                    CreatureType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Dexterity = table.Column<int>(type: "INTEGER", nullable: false),
                    DexteritySave = table.Column<int>(type: "INTEGER", nullable: false),
                    DMControl = table.Column<bool>(type: "INTEGER", nullable: false),
                    Features = table.Column<string>(type: "TEXT", nullable: true),
                    Hyperlink = table.Column<string>(type: "TEXT", nullable: true),
                    Immunity = table.Column<int>(type: "INTEGER", nullable: false),
                    InitiativeAdvantage = table.Column<bool>(type: "INTEGER", nullable: false),
                    InitiativeBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    Intelligence = table.Column<int>(type: "INTEGER", nullable: false),
                    IntelligenceSave = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelOrCR = table.Column<double>(type: "REAL", nullable: false),
                    MaxHP = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxHPString = table.Column<string>(type: "TEXT", nullable: true),
                    MaxLegendaryActions = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxLegendaryResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    Movement = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ProficiencyBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    Resistance = table.Column<int>(type: "INTEGER", nullable: false),
                    Senses = table.Column<string>(type: "TEXT", nullable: true),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    SpellSlots = table.Column<string>(type: "TEXT", nullable: true),
                    SpellStat = table.Column<int>(type: "INTEGER", nullable: false),
                    Strength = table.Column<int>(type: "INTEGER", nullable: false),
                    StrengthSave = table.Column<int>(type: "INTEGER", nullable: false),
                    Vulnerability = table.Column<int>(type: "INTEGER", nullable: false),
                    Wisdom = table.Column<int>(type: "INTEGER", nullable: false),
                    WisdomSave = table.Column<int>(type: "INTEGER", nullable: false),
                    XP = table.Column<int>(type: "INTEGER", nullable: false),
                    NotProficient = table.Column<int>(type: "INTEGER", nullable: false),
                    HalfProficient = table.Column<int>(type: "INTEGER", nullable: false),
                    Proficient = table.Column<int>(type: "INTEGER", nullable: false),
                    Expertise = table.Column<int>(type: "INTEGER", nullable: false),
                    ToolSkills = table.Column<string>(type: "TEXT", nullable: true),
                    Languages = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveEncounterCreatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AdjustedEncounterXP = table.Column<double>(type: "REAL", nullable: false),
                    CampaignId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsCampaignOnlyEncounter = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    ActiveTurnId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Log = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Encounters_ActiveEncounterCreatures_ActiveTurnId",
                        column: x => x.ActiveTurnId,
                        principalTable: "ActiveEncounterCreatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Encounters_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CreatureEncounter",
                columns: table => new
                {
                    CreaturesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EncounterId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureEncounter", x => new { x.CreaturesId, x.EncounterId });
                    table.ForeignKey(
                        name: "FK_CreatureEncounter_Creatures_CreaturesId",
                        column: x => x.CreaturesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureEncounter_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_ActiveEncounterCreatureId",
                table: "Abilities",
                column: "ActiveEncounterCreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_ActiveEncounterCreatureId1",
                table: "Abilities",
                column: "ActiveEncounterCreatureId1");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityCreature_CreatureId",
                table: "AbilityCreature",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveEncounterCreatures_ActiveEncounterId",
                table: "ActiveEncounterCreatures",
                column: "ActiveEncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveEncounterCreatures_ActiveEncounterId1",
                table: "ActiveEncounterCreatures",
                column: "ActiveEncounterId1");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureEncounter_EncounterId",
                table: "CreatureEncounter",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureParty_PartyId",
                table: "CreatureParty",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_ActiveTurnId",
                table: "Encounters",
                column: "ActiveTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_CampaignId",
                table: "Encounters",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_CampaignId",
                table: "Parties",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_ActiveEncounterCreatures_ActiveEncounterCreatureId",
                table: "Abilities",
                column: "ActiveEncounterCreatureId",
                principalTable: "ActiveEncounterCreatures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_ActiveEncounterCreatures_ActiveEncounterCreatureId1",
                table: "Abilities",
                column: "ActiveEncounterCreatureId1",
                principalTable: "ActiveEncounterCreatures",
                principalColumn: "Id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_ActiveEncounterCreatures_ActiveTurnId",
                table: "Encounters");

            migrationBuilder.DropTable(
                name: "AbilityCreature");

            migrationBuilder.DropTable(
                name: "CreatureEncounter");

            migrationBuilder.DropTable(
                name: "CreatureParty");

            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "Creatures");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "ActiveEncounterCreatures");

            migrationBuilder.DropTable(
                name: "Encounters");

            migrationBuilder.DropTable(
                name: "Campaigns");
        }
    }
}
