﻿// <auto-generated />
using System;
using EasyEncounters.Persistence.SQLLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EasyEncounters.Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240331033917_Encounter-Changes-Add-Count")]
    partial class EncounterChangesAddCount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("AbilityCreature", b =>
                {
                    b.Property<Guid>("AbilitiesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatureId")
                        .HasColumnType("TEXT");

                    b.HasKey("AbilitiesId", "CreatureId");

                    b.HasIndex("CreatureId");

                    b.ToTable("AbilityCreature");
                });

            modelBuilder.Entity("CreatureEncounter", b =>
                {
                    b.Property<Guid>("CreaturesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EncounterId")
                        .HasColumnType("TEXT");

                    b.HasKey("CreaturesId", "EncounterId");

                    b.HasIndex("EncounterId");

                    b.ToTable("CreatureEncounter");
                });

            modelBuilder.Entity("CreatureParty", b =>
                {
                    b.Property<Guid>("MembersId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PartyId")
                        .HasColumnType("TEXT");

                    b.HasKey("MembersId", "PartyId");

                    b.HasIndex("PartyId");

                    b.ToTable("CreatureParty");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.Ability", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("ActionSpeed")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ActiveEncounterCreatureId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CastTimeString")
                        .HasColumnType("TEXT");

                    b.Property<int>("CastingComponents")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Concentration")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DamageTypes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.Property<string>("EffectDescription")
                        .HasColumnType("TEXT");

                    b.Property<int>("MagicSchool")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaterialCost")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Resolution")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ResolutionStat")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SaveType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SpellLevel")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetAreaSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetAreaType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetDistance")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetDistanceType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeDuration")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeDurationType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ActiveEncounterCreatureId");

                    b.ToTable("Abilities");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Ability");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.ActiveEncounter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ActiveTurnId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Log")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ActiveTurnId");

                    b.ToTable("ActiveEncounters");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.ActiveEncounterCreature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AC")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ActiveConditions")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ActiveEncounterId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ActiveEncounterId1")
                        .HasColumnType("TEXT");

                    b.Property<string>("ActiveSpellSlots")
                        .HasColumnType("TEXT");

                    b.Property<int>("Alignment")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AttackDescription")
                        .HasColumnType("TEXT");

                    b.Property<int>("Charisma")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CharismaSave")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Concentrating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConditionImmunities")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Constitution")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConstitutionSave")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatureSubtype")
                        .HasColumnType("TEXT");

                    b.Property<int>("CreatureType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentHP")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentLegendaryActions")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentLegendaryResistance")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("DMControl")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Dead")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("Dexterity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DexteritySave")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("EncounterID")
                        .HasColumnType("TEXT");

                    b.Property<int>("EncounterMaxHP")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EncounterName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Expertise")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Features")
                        .HasColumnType("TEXT");

                    b.Property<int>("HalfProficient")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Hyperlink")
                        .HasColumnType("TEXT");

                    b.Property<int>("Immunity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Initiative")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InitiativeAdvantage")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InitiativeBonus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Intelligence")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IntelligenceSave")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Languages")
                        .HasColumnType("TEXT");

                    b.Property<double>("LevelOrCR")
                        .HasColumnType("REAL");

                    b.Property<int>("MaxHP")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaxHPString")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxLegendaryActions")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxLegendaryResistance")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Movement")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("NotProficient")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProficiencyBonus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Proficient")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Reaction")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Resistance")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Senses")
                        .HasColumnType("TEXT");

                    b.Property<int>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SpellSlots")
                        .HasColumnType("TEXT");

                    b.Property<int>("SpellStat")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Strength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StrengthSave")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TempHP")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ToolSkills")
                        .HasColumnType("TEXT");

                    b.Property<int>("Vulnerability")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Wisdom")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WisdomSave")
                        .HasColumnType("INTEGER");

                    b.Property<int>("XP")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ActiveEncounterId");

                    b.HasIndex("ActiveEncounterId1");

                    b.ToTable("ActiveEncounterCreatures");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.Campaign", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.Creature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AC")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Alignment")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AttackDescription")
                        .HasColumnType("TEXT");

                    b.Property<int>("Charisma")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CharismaSave")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConditionImmunities")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Constitution")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConstitutionSave")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatureSubtype")
                        .HasColumnType("TEXT");

                    b.Property<int>("CreatureType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("DMControl")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("Dexterity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DexteritySave")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Expertise")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Features")
                        .HasColumnType("TEXT");

                    b.Property<int>("HalfProficient")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Hyperlink")
                        .HasColumnType("TEXT");

                    b.Property<int>("Immunity")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InitiativeAdvantage")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InitiativeBonus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Intelligence")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IntelligenceSave")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Languages")
                        .HasColumnType("TEXT");

                    b.Property<double>("LevelOrCR")
                        .HasColumnType("REAL");

                    b.Property<int>("MaxHP")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaxHPString")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxLegendaryActions")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxLegendaryResistance")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Movement")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("NotProficient")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProficiencyBonus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Proficient")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Resistance")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Senses")
                        .HasColumnType("TEXT");

                    b.Property<int>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SpellSlots")
                        .HasColumnType("TEXT");

                    b.Property<int>("SpellStat")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Strength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StrengthSave")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ToolSkills")
                        .HasColumnType("TEXT");

                    b.Property<int>("Vulnerability")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Wisdom")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WisdomSave")
                        .HasColumnType("INTEGER");

                    b.Property<int>("XP")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Creatures");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.Encounter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("AdjustedEncounterXP")
                        .HasColumnType("REAL");

                    b.Property<Guid?>("CampaignId")
                        .HasColumnType("TEXT");

                    b.Property<int>("CreatureCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCampaignOnlyEncounter")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("Encounters");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.EncounterCreatures", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("CreatureId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("EncounterId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatureId");

                    b.HasIndex("EncounterId");

                    b.ToTable("EncounterCreatures");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.Party", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CampaignId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PartyDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PartyXPThresholds")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("Parties");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.ActiveAbility", b =>
                {
                    b.HasBaseType("EasyEncounters.Core.Models.Ability");

                    b.Property<Guid?>("ActiveEncounterCreatureId1")
                        .HasColumnType("TEXT");

                    b.Property<int>("ResolutionValue")
                        .HasColumnType("INTEGER");

                    b.HasIndex("ActiveEncounterCreatureId1");

                    b.HasDiscriminator().HasValue("ActiveAbility");
                });

            modelBuilder.Entity("AbilityCreature", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.Ability", null)
                        .WithMany()
                        .HasForeignKey("AbilitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasyEncounters.Core.Models.Creature", null)
                        .WithMany()
                        .HasForeignKey("CreatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CreatureEncounter", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.Creature", null)
                        .WithMany()
                        .HasForeignKey("CreaturesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasyEncounters.Core.Models.Encounter", null)
                        .WithMany()
                        .HasForeignKey("EncounterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CreatureParty", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.Creature", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasyEncounters.Core.Models.Party", null)
                        .WithMany()
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.Ability", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.ActiveEncounterCreature", null)
                        .WithMany("Abilities")
                        .HasForeignKey("ActiveEncounterCreatureId");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.ActiveEncounter", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.ActiveEncounterCreature", "ActiveTurn")
                        .WithMany()
                        .HasForeignKey("ActiveTurnId");

                    b.Navigation("ActiveTurn");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.ActiveEncounterCreature", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.ActiveEncounter", null)
                        .WithMany("ActiveCreatures")
                        .HasForeignKey("ActiveEncounterId");

                    b.HasOne("EasyEncounters.Core.Models.ActiveEncounter", null)
                        .WithMany("CreatureTurns")
                        .HasForeignKey("ActiveEncounterId1");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.Encounter", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.Campaign", "Campaign")
                        .WithMany()
                        .HasForeignKey("CampaignId");

                    b.Navigation("Campaign");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.EncounterCreatures", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.Creature", "Creature")
                        .WithMany()
                        .HasForeignKey("CreatureId");

                    b.HasOne("EasyEncounters.Core.Models.Encounter", null)
                        .WithMany("CreaturesByCount")
                        .HasForeignKey("EncounterId");

                    b.Navigation("Creature");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.Party", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.Campaign", "Campaign")
                        .WithMany()
                        .HasForeignKey("CampaignId");

                    b.Navigation("Campaign");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.ActiveAbility", b =>
                {
                    b.HasOne("EasyEncounters.Core.Models.ActiveEncounterCreature", null)
                        .WithMany("ActiveAbilities")
                        .HasForeignKey("ActiveEncounterCreatureId1");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.ActiveEncounter", b =>
                {
                    b.Navigation("ActiveCreatures");

                    b.Navigation("CreatureTurns");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.ActiveEncounterCreature", b =>
                {
                    b.Navigation("Abilities");

                    b.Navigation("ActiveAbilities");
                });

            modelBuilder.Entity("EasyEncounters.Core.Models.Encounter", b =>
                {
                    b.Navigation("CreaturesByCount");
                });
#pragma warning restore 612, 618
        }
    }
}
