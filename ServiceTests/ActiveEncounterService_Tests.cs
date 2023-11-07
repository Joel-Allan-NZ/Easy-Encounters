using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Services;
using NSubstitute;

namespace ServiceTests;
public class ActiveEncounterService_Tests : IDisposable
{
    ActiveEncounterService? _service;
    IRandomService randomService;
    IEncounterService encounterService;
    ICreatureService creatureService;
    ILogService logService;
    IDataService dataService;

    public ActiveEncounterService_Tests()
    {
        dataService = Substitute.For<IDataService>();
        randomService = Substitute.For<IRandomService>();
        encounterService = Substitute.For<IEncounterService>();
        creatureService = Substitute.For<ICreatureService>();
        logService = Substitute.For<ILogService>();

        _service = new ActiveEncounterService(randomService, dataService, encounterService, creatureService, logService);
    }

    public void Dispose()
    {
        _service = null;
        randomService = null;
        encounterService = null;
        creatureService = null;
        logService = null;
        dataService = null;
    }

    [Fact]
    public void CreateActiveEncounterAsync_NotNull()
    {
        Encounter encounter = new();
        Party party = new();
        dataService.SaveAddAsync<ActiveEncounter>(default).ReturnsForAnyArgs(Task.CompletedTask);
        encounterService.CreateActiveEncounterCreature(default, default).ReturnsForAnyArgs(new ActiveEncounterCreature());

        var active = _service.CreateActiveEncounterAsync(encounter, party);

        Assert.NotNull(active);
    }

    [Fact]
    public void CreateActiveEncounterAsync_EncounterCreatures_NotNull()
    {
        Encounter encounter = new()
        {
            Creatures = new List<Creature>()
            {
                new Creature(),
                new Creature()
            }
        };
        Party party = new();
        dataService.SaveAddAsync<ActiveEncounter>(default).ReturnsForAnyArgs(Task.CompletedTask);
        encounterService.CreateActiveEncounterCreature(default, default).ReturnsForAnyArgs(new ActiveEncounterCreature());

        var active = _service.CreateActiveEncounterAsync(encounter, party);

        Assert.NotNull(active);
    }


    [Fact]
    public void CreateActiveEncounterAsync_PartyCreatures_NotNull()
    {
        Encounter encounter = new();
        Party party = new()
        {
            Members = new List<Creature>()
            {
                new Creature(),
                new Creature()
            }
        };
        dataService.SaveAddAsync<ActiveEncounter>(default).ReturnsForAnyArgs(Task.CompletedTask);
        encounterService.CreateActiveEncounterCreature(default, default).ReturnsForAnyArgs(new ActiveEncounterCreature());

        var active = _service.CreateActiveEncounterAsync(encounter, party);

        Assert.NotNull(active);
    }

    [Fact]
    public void CreateActiveEncounterAsync_PartyAndEncounterCreatures_NotNull()
    {
        Encounter encounter = new()
        {
            Creatures = new List<Creature>()
            {
                new Creature(),
                new Creature()
            }
        };
        Party party = new()
        {
            Members = new List<Creature>()
            {
                new Creature(),
                new Creature()
            }
        };
        dataService.SaveAddAsync<ActiveEncounter>(default).ReturnsForAnyArgs(Task.CompletedTask);
        encounterService.CreateActiveEncounterCreature(default, default).ReturnsForAnyArgs(new ActiveEncounterCreature());

        var active = _service.CreateActiveEncounterAsync(encounter, party);

        Assert.NotNull(active);
    }

    [Fact]
    public void AddEncounterCreature_NameIncremented()
    {
        ActiveEncounter activeEncounter = new()
        {
            ActiveCreatures = new()
            {
                new ActiveEncounterCreature()
                {
                    EncounterName = "Test", Name="Test"
                },
                new ActiveEncounterCreature()
                {
                    EncounterName = "Test 1", Name = "Test"
                },
                new ActiveEncounterCreature()
                {
                    EncounterName = "Testy", Name="Testy"
                },
                new ActiveEncounterCreature()
                {
                    EncounterName = "test", Name="test"
                }
            }
        };
        Creature creature = new();
        bool hp = false;

        encounterService.CreateActiveEncounterCreature(default, default).ReturnsForAnyArgs(new ActiveEncounterCreature() { Name="Test"});

        _service.AddEncounterCreature(activeEncounter, creature, hp);

        Assert.Equal("Test 2", activeEncounter.ActiveCreatures.Last().EncounterName);

    }

    [Fact]
    public void AddEncounterCreature_NameNotIncremented()
    {

        ActiveEncounter activeEncounter = new()
        {
            ActiveCreatures = new()
            {
                new ActiveEncounterCreature()
                {
                    EncounterName = "Teste", Name = "Teste"
                },
                new ActiveEncounterCreature()
                {
                    EncounterName = "Testy", Name = "Testy"
                },
                new ActiveEncounterCreature()
                {
                    EncounterName = "test", Name = "test"
                }
            }
        };
        Creature creature = new();
        bool hp = false;

        encounterService.CreateActiveEncounterCreature(default, default).ReturnsForAnyArgs(new ActiveEncounterCreature() { EncounterName = "Test", Name="Test" });

        _service.AddEncounterCreature(activeEncounter, creature, hp);

        Assert.Equal("Test", activeEncounter.ActiveCreatures.Last().EncounterName);
    }


}
