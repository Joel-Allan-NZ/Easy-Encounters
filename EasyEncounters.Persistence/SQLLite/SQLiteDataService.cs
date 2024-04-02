using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace EasyEncounters.Persistence.SQLLite;
public class SQLiteDataService : IDataService
{
    private readonly DataContext _dbContext;
    private static readonly string jsonFile = @"TextJson.txt";
    private static readonly string logFile = @"LogTextJson.txt";
    private readonly IAbilityService _abilityService;
    private readonly ICreatureService _creatureService;
    private readonly IModelOptionsService _modelOptionsService;
    private readonly IFileService _fileService;



    public SQLiteDataService(DataContext context, IFileService fileService, IAbilityService abiltyService, ICreatureService creatureService, IModelOptionsService modelOptionsService)
    {
        _fileService = fileService;
        _creatureService = creatureService;
        _abilityService = abiltyService;
        _modelOptionsService = modelOptionsService;

        _dbContext = context;

        //_dbContext.Campaigns.Load();
        //_dbContext.Encounters.Load();
        //_dbContext.Abilities.Load();
        //_dbContext.ActiveAbilities.Load();
        //_dbContext.ActiveEncounterCreatures.Load();
        //_dbContext.ActiveEncounters.Load();
        //_dbContext.Parties.Load();
        

    }
    public IQueryable<Campaign> Campaigns() => _dbContext.Campaigns;
    public IQueryable<Encounter> Encounters() => _dbContext.Encounters;
    public IQueryable<Creature> Creatures() => _dbContext.Creatures;
    public IQueryable<Ability> Abilities() => _dbContext.Abilities;
    public IQueryable<Party> Parties() => _dbContext.Parties;

    public IQueryable<ActiveEncounter> ActiveEncounters() => _dbContext.ActiveEncounters;

    public async Task<T> CopyAsync<T>(T entity) where T : IPersistable
    {
        IPersistable result;
        if (entity is Encounter encounter)
        {
            result = new Encounter(encounter.Name, new List<Creature>(encounter.Creatures), encounter.Description, encounter.AdjustedEncounterXP, encounter.Campaign, encounter.IsCampaignOnlyEncounter);
        }
        else if (entity is Creature creature)
        {
            result = _creatureService.DeepCopy(creature);
        }
        else if (entity is Campaign campaign)
        {
            result = new Campaign(campaign.Name, campaign.Description);
        }
        else if (entity is Party party)
        {
            result = new Party(party.Campaign, party.Name, new(party.Members), (double[])party.PartyXPThresholds.Clone(), party.PartyDescription);
        }
        else if (entity is Ability ability)
        {
            result = CopyAbility(ability);
        }
        else
        {
            throw new NotImplementedException($"Copying {typeof(T)} is not yet implemented.");
        }

        await _dbContext.SaveChangesAsync();
        return (T)result;
    }

    private IPersistable CopyAbility(Ability ability)
    {
        var result = new Ability();
        _abilityService.CopyTo(result, ability);
        return result;
    }


    public async Task DeleteAsync<T>(T entity) where T : IPersistable
    {
        if(entity is ActiveEncounter activeEncounter)
        {
            activeEncounter.ActiveCreatures.Clear();
            var toDelete = activeEncounter.CreatureTurns;
            activeEncounter.CreatureTurns.Clear();
            foreach(var activeEncounterCreature in toDelete)
            {
                _dbContext.Remove(activeEncounterCreature);
            }
        }

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<Campaign>> GetAllCampaignsAsync() => await _dbContext.Campaigns.ToListAsync();
    public async Task<IEnumerable<Creature>> GetAllCreaturesAsync() => await _dbContext.Creatures.Include(x => x.Abilities).ToListAsync();
    public async Task<IEnumerable<Encounter>> GetAllEncountersAsync() => await _dbContext.Encounters.Include(x=> x.Creatures).ToListAsync();
    public async Task<IEnumerable<Party>> GetAllPartiesAsync() => await _dbContext.Parties.Include(x => x.Members).ThenInclude(x => x.Abilities).ToListAsync();
    public async Task<IEnumerable<Ability>> GetAllSpellsAsync() => await _dbContext.Abilities.Where(x => x.SpellLevel != Core.Models.Enums.SpellLevel.NotASpell).ToListAsync();

    public async Task<IEnumerable<Encounter>> GetCampaignEncountersAsync(Campaign campaign, bool includeGeneralEncounters)
    {
        if (includeGeneralEncounters)
        {
            return await _dbContext.Encounters.Where(x => x.Campaign == null || x.Campaign.Equals(campaign) || !x.IsCampaignOnlyEncounter).Include(x => x.Creatures).ThenInclude(x => x.Abilities).ToListAsync();
        }
        else
        {
            return await _dbContext.Encounters.Where(x => x.Campaign == null || x.Campaign.Equals(campaign)).Include(x => x.Creatures).ThenInclude(x => x.Abilities).ToListAsync();
        }
    }
    public async Task<IEnumerable<Party>> GetCampaignPartiesAsync(Campaign campaign) => await _dbContext.Parties.Where(x => x.Campaign == campaign).Include(x => x.Members).ThenInclude(x => x.Abilities).ToListAsync();
    public async Task SaveAddAsync<T>(T entity) where T : class, IPersistable
    {
        //if (entity is ActiveEncounter)
        //{
        //    return;
        //}
        //else
        //{


            var first = (T?)await _dbContext.FindAsync(typeof(T), entity.Id);
            if (first == null)
            {
                _dbContext.Add<T>(entity);
            }
            else
            {
                _dbContext.Entry(first).CurrentValues.SetValues(entity);
            }
        //}
        //otherwise it's an existing (presumably) entity, that's already been marked modified. Might be something to watch when not wishing to save changes? TODO: investigate this behavior.
        await _dbContext.SaveChangesAsync();
    }

    private async Task SaveAddCreatureAsync(Creature entity)
    {
        var existing = await _dbContext.Creatures.FindAsync(entity.Id);
        if(existing == null)
        {
            _dbContext.Add(entity);
        }


    }

    private async Task SaveAddEncounterAsync(Encounter entity)
    {

    }

    private async Task SaveAddPartyAsync(Party entity)
    {
    }

    private async Task SaveAddCampaignAsync(Campaign entity)
    {
    }

    private async Task SaveAddAbilityAsync(Ability entity)
    {
    }

    public async Task SaveAddAsync<T>(IEnumerable<T> entities) where T : IPersistable
    {
        //await _dbContext.AddRangeAsync(entities.ToArray());
        foreach (var entity in entities)
        {
            var first = await _dbContext.FindAsync(typeof(T), entity.Id);
            if (first == null)
            {
                _dbContext.Add(entity);
            }
            else
            {
                _dbContext.Entry(first).CurrentValues.SetValues(entity);
                
                //if(entity is Creature creature)
                //{
                //    if(((Creature)first).Abilities == null)
                //    {
                //        ((Creature)first).Abilities = new();
                //    }
                //    foreach(var ability in creature.Abilities)
                //    {
                //        var abilityFromID = _dbContext.Abilities.Find(ability.Id);


                //        ((Creature)first).Abilities.Add(abilityFromID);
                //        _dbContext.Entry(first).State = EntityState.Modified;
                //    }
                //}

                //else if(entity is Encounter encounter)
                //{

                //    if (((Encounter)first).Creatures == null)
                //    {
                //        ((Encounter)first).Creatures = new();
                //    }
                //    foreach(var critter in encounter.Creatures)
                //    {
                //        var critterFromID = _dbContext.Creatures.Find(critter.Id);
                //        ((Encounter)first).Creatures.Add(critterFromID);

                //        _dbContext.Entry(first).State = EntityState.Modified;
                //    }
                //}

                //else if(entity is Party party)
                //{
                //    ((Party)first).Members = new();

                //    foreach (var member in party.Members)
                //    {
                //        var memberFromID = _dbContext.Creatures.Find(member.Id);

                //        ((Party)first).Members.Add(memberFromID);

                //        _dbContext.Entry(first).State = EntityState.Modified;
                //    }
                //}
                
            }
            
        }



        //await _dbContext.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }
    public async Task WriteLogAsync(IEnumerable<string> log) => await _fileService.AppendAsync(_modelOptionsService.SavePath, logFile, log);

    public async Task CommitChanges()
    {
        _dbContext.SaveChanges();
    }
}
