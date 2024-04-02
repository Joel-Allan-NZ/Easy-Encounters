//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Management;
//using System.Text;
//using System.Threading.Tasks;
//using EasyEncounters.Core.Contracts.Services;
//using EasyEncounters.Core.Models;
//using EasyEncounters.Core.Services;
//using EasyEncounters.Services;

//namespace EasyEncounters.Persistence.ApiToModel;
//public static class ReadDNDJson
//{
//    public async static Task<List<ApiSpell>> ReadSpells()
//    {
//        var folderLocation = @"C:\Users\joelc\Downloads\";
//        var fileName = @"5e-SRD-Spells.json";

//        var fileService = new FileService();

//        return await fileService.ReadAsyncUTF<List<ApiSpell>>(folderLocation, fileName);

//    }

//    public async static Task<List<Ability>> ReadAbilities()
//    {
//        var spells = await ReadSpells();

//        var converted = spells.Select(x => Converter.ConvertToAbility(x)).ToList();

//        return converted;
//    }

//    private async static Task<List<ApiCreature>> ReadAPICreatures()
//    {
//        var folderLocation = @"C:\Users\joelc\Downloads\";
//        var fileName = @"5e-SRD-Monsters.json";

//        var fileService = new FileService();

//        return await fileService.ReadAsyncUTF<List<ApiCreature>>(folderLocation, fileName);
//    }

//    public async static Task<List<Creature>> ReadCreatures(List<Ability> spells, ICreatureService creatureService)
//    {
//        var apiCreatures = await ReadAPICreatures();

//        var converted = apiCreatures.Select(x => Converter.ConvertToCreature(x, spells, creatureService)).ToList();

//        return converted;
//    }
//}
