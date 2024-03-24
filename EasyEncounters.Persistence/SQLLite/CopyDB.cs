using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Services;

namespace EasyEncounters.Persistence.SQLLite;
public class CopyDB
{
    private IDataService old;
    private IDataService sql;
    public CopyDB(IDataService sqljj, IFileService fileService, IAbilityService abilityService, ICreatureService creatureService, IModelOptionsService modelOptionsService)
    {
        old = new DataService(fileService, abilityService, creatureService, modelOptionsService);
        sql = sqljj;
        //sql = new SQLiteDataService(fileService, abilityService, creatureService, modelOptionsService);
    }

    public async void CopyAll()
    {
        var oldCampaigns = await old.GetAllCampaignsAsync();
        List<Creature> oldCreatures = (List<Creature>)await old.GetAllCreaturesAsync();
        var oldParties = await old.GetAllPartiesAsync();
        var oldAbilities = await old.GetAllSpellsAsync();
        var oldEncounters = await old.GetAllEncountersAsync();
        foreach(var party in oldParties)
        {
            party.PartyDescription ??= "";
        }
        //var oldData = new List<object>();
        //oldData.AddRange(oldCampaigns);
        //oldData.AddRange(oldCreatures);
        //oldData.AddRange(oldParties);
        //oldData.AddRange(oldAbilities);
        //oldData.AddRange(oldEncounters);

        //foreach (var entity in oldCreatures)
        //{
        //    await sql.SaveAddAsync(entity);
        //}
        await sql.SaveAddAsync(oldCreatures.ToList());
        await sql.SaveAddAsync(oldCampaigns.ToList());

        await sql.SaveAddAsync(oldParties.ToList());
        await sql.SaveAddAsync(oldAbilities.ToList());
        await sql.SaveAddAsync(oldEncounters.ToList());

        await ((SQLiteDataService)sql).CommitChanges();
    }
    public async void CopyCampaigns()
    {
        var oldCampaigns = await old.GetAllCampaignsAsync();
        foreach (var entity in oldCampaigns)
        {
            await sql.SaveAddAsync(entity);
        }
    }

    public async void CopyCreatures()
    {
        var oldCreatures = await old.GetAllCreaturesAsync();
        foreach (var entity in oldCreatures)
        {
            await sql.SaveAddAsync((Creature)entity);
        }
    }

    public async void CopyParties()
    {
        var oldParties = await old.GetAllPartiesAsync();
        foreach (var entity in oldParties)
        {
            await sql.SaveAddAsync(entity);
        }
    }
    public async void CopyAbilities()
    {

        var oldAbilities = await old.GetAllSpellsAsync();





        foreach (var entity in oldAbilities)
        {
            await sql.SaveAddAsync(entity);
        }

    }
    public async void CopyEncounters()
    {
        var oldEncounters = await old.GetAllEncountersAsync();
        foreach (var entity in oldEncounters)
        {
            await sql.SaveAddAsync(entity);
        }
    }


}
