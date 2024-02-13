﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Services;
public class DataService : IDataService
{
    private Data _cached;
    private readonly IFileService _fileService;
    private static readonly string folderPath = @"D:\D&D\DND Tools"; //todo: make setting
    private readonly IEncounterService _encounterService;
    private static readonly string jsonFile = @"TextJson.txt";
    private static readonly string logFile = @"LogTextJson.txt";

    private readonly IAbilityService _abilityService;
    private readonly ICreatureService _creatureService;

    public DataService(IFileService fileService, IEncounterService encounterService, IAbilityService abiltyService, ICreatureService creatureService)
    {
        _encounterService = encounterService;
        _fileService = fileService;
        _creatureService = creatureService;
        _abilityService = abiltyService;
        _cached = _fileService.Read<Data>(folderPath, jsonFile);
    }

    public async Task<bool> ActiveEncounterExistsAsync()
    {
        await RefreshCacheAsync();

        return _cached?.ActiveEncounter != null;
    }

    public async Task<ActiveEncounter> GetActiveEncounterAsync()
    {
        await RefreshCacheAsync();

        return _cached?.ActiveEncounter;
    }

    public async Task<IEnumerable<Campaign>> GetAllCampaignsAsync()
    {
        await RefreshCacheAsync();

        return _cached?.Campaigns;
    }
    public async Task<IEnumerable<Creature>> GetAllCreaturesAsync()
    {
        await RefreshCacheAsync();

        return _cached?.Creatures;
    }
    public async Task<IEnumerable<Encounter>> GetAllEncountersAsync()
    {
        await RefreshCacheAsync();

        return _cached?.Encounters;
    }
    public async Task<IEnumerable<Party>> GetAllPartiesAsync()
    {
        await RefreshCacheAsync();

        return _cached?.Parties;
    }
    public async Task<IEnumerable<Ability>> GetAllSpellsAsync()
    {
        await RefreshCacheAsync();

        return _cached?.Abilities.Where(x => x.SpellLevel != Models.Enums.SpellLevel.NotASpell);
    }
    public async Task WriteLog(IEnumerable<string> log)
    {
        await _fileService.AppendAsync(folderPath, logFile, log);
    }

    public async Task<IEnumerable<Party>> GetCampaignPartiesAsync(Campaign campaign)
    {
        await RefreshCacheAsync();
        return _cached?.Parties.Where(x => x.Campaign == campaign);
    }

    private async Task RefreshCacheAsync()
    {
        _cached ??= await _fileService.ReadAsync<Data>(folderPath, jsonFile);
    }

    public async Task<IEnumerable<EncounterData>> GetAllEncounterDataAsync(Party party)
    {
        List<EncounterData> data = new();
        var partyXPThreshold = _encounterService.GetPartyXPThreshold(party);

        await RefreshCacheAsync();
        foreach(var encounter in await GetAllEncountersAsync())
        {
            data.Add(new EncounterData(encounter, party, _encounterService.DetermineDifficultyForParty(encounter, partyXPThreshold)));
        }
        return data;

    }

    private void SaveAddCreature(Creature creature)
    {
        var cachedCreature = _cached.Creatures.FirstOrDefault(x => x.Equals(creature));
        if(cachedCreature != null)
        {
            _creatureService.CopyTo(cachedCreature, creature);
        }
        else
            _cached.Creatures.Add(creature);

        //if (_cached.Creatures.Contains(creature))
        //{
        //    var cachedCreature = _cached.Creatures.First(x => x.Equals(creature));
        //    _creatureService.CopyTo(cachedCreature, creature);
        //    //cachedCreature.CopyFrom(creature);
        //}
        //else
        //    _cached.Creatures.Add(creature);
    }

    private void SaveAddEncounter(Encounter encounter)
    {
        if (_cached.Encounters.Contains(encounter))
        {
            var cachedEnc = _cached.Encounters.First(x => x.Equals(encounter));

            cachedEnc.Creatures = encounter.Creatures;
            cachedEnc.Description = encounter.Description;
            cachedEnc.Name = encounter.Name;
        }
        else
        {
            _cached.Encounters.Add(encounter);
        }
        foreach (var creature in encounter.Creatures)
            SaveAddCreature(creature);
    }

    private void SaveAddParty(Party party)
    {
        if (_cached.Parties.Contains(party))
        {
            var cachedParty = _cached.Parties.First(x => x.Equals(party));

            cachedParty.Name = party.Name;
            cachedParty.Members = party.Members;
            cachedParty.Campaign = party.Campaign;

        }
        else
            _cached.Parties.Add(party);

        foreach (var member in party.Members)
            SaveAddCreature(member);
    }

    private void SaveAddActiveEncounter(ActiveEncounter activeEncounter)
    {
        _cached.ActiveEncounter = activeEncounter;
    }

    private void SaveAddAbility(Ability ability)
    {
        if (_cached.Abilities.Contains(ability))
        {
            var cachedAbility = _cached.Abilities.First(x => x.Equals(ability));

            _abilityService.CopyTo(cachedAbility, ability);
        }
        else if (ability.SpellLevel != Models.Enums.SpellLevel.NotASpell)
            _cached.Abilities.Add(ability);
    }

    private void SaveAddCampaign(Campaign campaign)
    {
        if (_cached.Campaigns.Contains(campaign))
        {
            var cachedCampaign = _cached.Campaigns.First(x => x.Equals(campaign));
            cachedCampaign.Description = campaign.Description;
            cachedCampaign.Name = campaign.Name;
        }
        else
            _cached.Campaigns.Add(campaign);
    }
    //TODO: investigate if this is entirely pointless - is there any place where we're dealing with a cloned entity rather than a straight reference?
    //todo: make some kind of persistable base type etc
    public async Task SaveAddAsync<T>(T entity) where T:IPersistable
    {
        if(entity is ActiveEncounter)
        {
            SaveAddActiveEncounter(entity as ActiveEncounter);        
            
        }
        else if(entity is Encounter)
        {

            SaveAddEncounter(entity as Encounter);
        }
        else if(entity is Party)
        {
            SaveAddParty(entity as Party);
        }
        else if(entity is Creature)
        {
            SaveAddCreature(entity as Creature);

        }
        else if(entity is Campaign)
        {
            SaveAddCampaign(entity as Campaign);
        }
        else if (entity is Ability)
        {
            SaveAddAbility(entity as Ability);
        }
        else
        {
            return;
        }
        await _fileService.SaveAsync(folderPath, jsonFile, _cached);
    }

    public async Task ClearActiveEncounterAsync()
    {
        await RefreshCacheAsync();
        _cached.ActiveEncounter = null;

        await _fileService.SaveAsync(folderPath, jsonFile, _cached);
    }

 
    public async Task DeleteAsync<T>(T entity) where T:IPersistable
    {
        await RefreshCacheAsync();

        if (entity is ActiveEncounter)
            DeleteActiveEncounter(entity as ActiveEncounter);
        else if (entity is Encounter)
            DeleteEncounter(entity as Encounter);
        else if (entity is Party)
            DeleteParty(entity as Party);
        else if (entity is Campaign)
            DeleteCampaign(entity as Campaign);
        else if (entity is Creature)
            DeleteCreature(entity as Creature);
        else if (entity is Ability)
            DeleteSpell(entity as Ability);

        await _fileService.SaveAsync(folderPath, jsonFile, _cached);
    }

    public async Task<T> CopyAsync<T>(T entity) where T : IPersistable
    {
        await RefreshCacheAsync();
        var tType = typeof(T);
        T result;

        if (tType == typeof(Encounter))
            result = (T)CopyEncounter(entity as Encounter);
        else if (entity is Creature)
            result = (T)CopyCreature(entity as Creature);
        else if (entity is Campaign)
            result = (T)CopyCampaign(entity as Campaign);
        else if (entity is Party)
            result = (T)CopyParty(entity as Party);
        else if (entity is Ability)
            result = (T)CopySpell(entity as Ability);
        else
            throw new ArgumentException("Type of T must be an IPersistable"); //should never happen as ActiveEncounters are Encounters too.

        await _fileService.SaveAsync(folderPath, jsonFile, _cached);
        return result;
    }

    private IPersistable CopyEncounter(Encounter encounter)
    {
        var res = new Encounter(encounter.Name, new List<Creature>(encounter.Creatures), encounter.Description);
        _cached.Encounters.Add(res);
        return res;
    }

    private IPersistable CopyCreature(Creature creature)
    {
        //todo: all copy functions with a proper factory set up
        var res = new Creature();
        _creatureService.CopyTo(res, creature);
        _cached.Creatures.Add(res);
        return res;
    }

    private IPersistable CopyCampaign(Campaign campaign)
    {
        var res = new Campaign(campaign.Name, campaign.Description);
        _cached.Campaigns.Add(res);
        return res;
    }

    private IPersistable CopyParty(Party party)
    {
        var res = new Party(party.Campaign, party.Name, new List<Creature>(party.Members));
        _cached.Parties.Add(res);
        return res;
    }

    private IPersistable CopySpell(Ability ability)
    {
        var res = new Ability();
        _abilityService.CopyTo(res, ability);
        return res;
    }

    private void DeleteCreature(Creature creature)
    {
        //removes creature from list of creatures, but doesn't attempt to remove it from a party or encounter it might exist in.
        //those entries will still have a reference to the creature which will still exist as an object thanks to how the json
        //is persisting entries. This is less of a headache than accidentally removing a creature and having it removed from places it's being used...
        //long term solution is to prompt user with a "do you want to delete this creature being used in x locations" dialog, but out of scope for now.
        _cached.Creatures.Remove(creature);
        
    }

    private void DeleteParty(Party party)
    {
        _cached.Parties.Remove(party);
    }

    private void DeleteCampaign(Campaign campaign)
    {
        //removes campaign from list of campaigns, but doesn't attempt to remove it from a party.
        //those entries will still have a reference to the campaign which will still exist as an object thanks to how the json
        //is persisting entries. This is less of a headache than accidentally removing a campaign and having it removed from places it's being used...
        //long term solution is to prompt user with a "do you want to delete this campaign being used in x locations" dialog, but out of scope for now.
        _cached.Campaigns.Remove(campaign);

    }

    private void DeleteSpell(Ability ability)
    {
        _cached.Abilities.Remove(ability);
    }


    private void DeleteEncounter(Encounter encounter)
    {
        _cached.Encounters.Remove(encounter);
    }

    public void DeleteActiveEncounter(ActiveEncounter activeEncounter)
    {
        _cached.ActiveEncounter = null;
    }
}
