//using System;
//using System.CodeDom;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Security.AccessControl;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using System.Xml.XPath;
//using EasyEncounters.Core.Contracts.Services;
//using EasyEncounters.Core.Models;
//using EasyEncounters.Core.Models.Enums;
//using EasyEncounters.Core.Services;
//using static System.Collections.Specialized.BitVector32;

//namespace EasyEncounters.Persistence.ApiToModel;
//public static class Converter
//{
//    public static Creature ConvertToCreature(ApiCreature creature, List<Ability> spells, ICreatureService creatureService)
//    {
//        Creature result = new();
//        //result.SpellSlots = new();
//        result.Strength = creature.strength;
//        result.Charisma = creature.charisma;
//        result.Dexterity = creature.dexterity;
//        result.Wisdom = creature.wisdom;
//        result.Intelligence = creature.intelligence;
//        result.Constitution = creature.constitution;
//        result.ProficiencyBonus = creature.proficiency_bonus;

//        result.Abilities = new();

//        result.AC = GetAC(creature);
//        result.Alignment = GetAlignment(creature);
//        result.AttackDescription = GetAttackDescription(creature);
        
//        GetAbilities(result, creature, spells);
//        ResolveProficiencies(result, creature, creatureService); //also manages saves.
        
//        result.Description = "";
//        result.DMControl = true;
//        List<string> spellNames = new();

//        //result.Features = ResolveFeatures(result, creature, out spellNames); //needs to also handle spells and shiiiiiit

//        if (spellNames != null && spellNames.Count > 0)
//        {

//            foreach (var spellName in spellNames)
//            {
//                var match = spells.FirstOrDefault(x => x.Name == spellName);
//                if (match != null)
//                    result.Abilities.Add(match);
//            }
//        }

//        //todo: spell slots


//        result.Senses = GetSenses(creature);

//        result.Languages = GetLanguages(creature);

//        result.LevelOrCR = creature.challenge_rating;
//         //from inspection:
//        result.InitiativeAdvantage = false;
//        result.InitiativeBonus = 0;

//        result.MaxHP = creature.hit_points;
//        result.MaxHPString = creature.hit_points_roll;

//        result.Movement = GetMovement(creature.speed);

//        result.Name = creature.name;

//        result.Size = GetSize(creature);

//        result.CreatureSubtype = creature.subtype;
//        result.CreatureType = GetCreatureType(creature.type);
//        result.ToolSkills = "";
        

//        result.Hyperlink = "https://www.dnd5eapi.co" + creature.url;


//        ResolveImmunitiesVuln(result, creature);

//        return result;

//    }

//    private static string? GetSenses(ApiCreature creature)
//    {
//        var result = "";
//        if(creature.senses != null && creature.senses.Keys.Count > 0)
//        {
//            foreach(var sense in creature.senses.Keys)
//            {
//                result += $"{sense} {creature.senses[sense]}, ";
//            }
//        }
//        return result[0..^2];
//    }

//    private static string GetLanguages(ApiCreature creature)
//    {
//        return creature.languages ?? "";
//    }
//    private static CreatureSizeClass GetSize(ApiCreature creature)
//    {
//        if(Enum.TryParse(typeof(CreatureSizeClass), creature.size, true, out var size))
//        {
//            return (CreatureSizeClass)size;
//        }
//        return CreatureSizeClass.Unknown;
//    }

//    private static string GetMovement(Dictionary<string, string> speed)
//    {
//        var speedString = "";
//        if(speed != null && speed.Keys.Count > 0)
//        {
//            if (speed.ContainsKey("walk"))
//            {
//                speedString += speed["walk"];
//            }
//            foreach (var key in speed.Keys)
//            {
//                if (key != "walk")
//                {
//                    speedString += $", {key} {speed[key]}";
//                }
//            }
//        }
//        return speedString;
//    }
//    private static CreatureType GetCreatureType(string type)
//    {
//        if(Enum.TryParse(typeof(CreatureType), type, true, out var creatureType))
//        {
//            return (CreatureType)creatureType;
//        }
//        return CreatureType.Any;
//    }
//    private static void ResolveImmunitiesVuln(Creature result, ApiCreature creature)
//    {
//        result.Immunity = ResolveDamageTypeStat(creature.damage_immunities);
//        result.Vulnerability = ResolveDamageTypeStat(creature.damage_vulnerabilities);
//        result.Resistance = ResolveDamageTypeStat(creature.damage_resistances);

//        result.ConditionImmunities = ResolveConditions(creature.condition_immunities.Select(x => x["name"]).ToList());

//    }


//    private static DamageType ResolveDamageTypeStat(List<string> enumStrings)
//    {
//        DamageType value = default(DamageType);

//        if (enumStrings != null)
//        {
//            foreach (var enumString in enumStrings)
//            {
//                if (Enum.TryParse(typeof(DamageType), enumString, true, out var result))
//                {
//                    value |= (DamageType)result;
//                }
//            }
//        }
//        return value;
//    }

//    private static Condition ResolveConditions(List<string> conditionStrings)
//    {
//        Condition value = default(Condition);

//        if (conditionStrings != null)
//        {
//            foreach (var conditionString in conditionStrings)
//            {
//                if (Enum.TryParse(typeof(Condition), conditionString, true, out var result))
//                {
//                    value |= (Condition)result;
//                }
//            }
//        }
//        return value;
//    }

//    //private static string ResolveFeatures(Creature result, ApiCreature creature, out List<string> spellNames)
//    //{
//    //    spellNames = new List<string>();
//    //    result.SpellStat = CreatureAttributeType.None;

//    //    string features = "";

//    //    if(creature.special_abilities != null)
//    //    {
//    //        foreach(var specialAbility in creature.special_abilities)
//    //        {
//    //            if ((string)specialAbility["name"] != "Innate Spellcasting" && (string)specialAbility["name"] != "Spellcasting")
//    //            {
//    //                features += ($"{(string)specialAbility["name"]}. {(string)specialAbility["desc"]}\n\n");

//    //                if (((string)specialAbility["name"]).Contains("Legendary"))
//    //                {
//    //                    result.MaxLegendaryResistance = 3;
//    //                }
//    //            }
//    //            else
//    //            {
//    //                var spellcastingDict = (dynamic)specialAbility["spellcasting"];
//    //                var spellCastingStatStringDict = spellcastingDict["ability"];

//    //                if (spellcastingDict.ContainsKey("slots"))
//    //                {
//    //                    var slots = spellcastingDict["slots"];
//    //                    result.SpellSlots = "";
//    //                    foreach(var kvp in slots)
//    //                    {
//    //                        //var key = (KeyValuePair<string, int>)kvp;
//    //                        var slotLevel = int.Parse((string)kvp.Name);

//    //                        result.SpellSlots[slotLevel] = Convert.ToInt32(kvp.First);
//    //                    }
//    //                }

//    //                var spellCastingStatString = (string)spellCastingStatStringDict["name"];

//    //                result.SpellStat = GetCreatureAttributeFromPartialString(spellCastingStatString);

//    //                var spellList = SpellNames(spellcastingDict);

//    //                foreach(var spellName in spellList)
//    //                {
//    //                    spellNames.Add(spellName);
//    //                }

//    //            }
//    //        }
//    //    }
//    //    return features;
//    //}

//    private static List<string> SpellNames(Dictionary<string, object> spellcastingDict)
//    {
//        var result = new List<string>();

//        if (spellcastingDict.ContainsKey("spells"))
//        {
//            var spells = (List<Dictionary<string, object>>)spellcastingDict["spells"];

//            foreach(var spell in spells)
//            {
//                string name = (string)spell["name"];

//                result.Add(name);
//            }
//        }
//        return result;

//    }

//    private static List<string> SpellNames(dynamic spellCastingDict)
//    {
//        var result = new List<string>();

//        if (spellCastingDict.ContainsKey("spells"))
//        {
//            var spells = spellCastingDict["spells"];

//            foreach (var spell in spells)
//            {
//                var name = (string)spell["name"];

//                result.Add(name);
//            }
//        }
//        return result;
//    }

//    private static void ResolveProficiencies(Creature result, ApiCreature creature, ICreatureService creatureService)
//    {
//        result.StrengthSave = (result.Strength - 10) / 2;
//        result.DexteritySave = (result.Dexterity - 10) / 2;
//        result.ConstitutionSave = (result.Constitution - 10) / 2;
//        result.WisdomSave = (result.Wisdom - 10) / 2;
//        result.IntelligenceSave = (result.Intelligence - 10) / 2;
//        result.CharismaSave = (result.CharismaSave - 10) / 2;

//        if (creature.proficiencies != null && creature.proficiencies.Count > 0)
//        {
//            foreach (var proficiency in creature.proficiencies)
//            {
//                int value = Convert.ToInt32((long)proficiency["value"]);

//                var data = (dynamic)proficiency["proficiency"];

//                var name = ((string)data["name"]).Split(' ').Last();
//                var typeString = ((string)data["index"]).Split('-').First();

//                if (typeString == "saving")
//                {
//                    ResolveSavingProficiency(result, name, value);
//                }
//                else if (typeString == "skill")
//                {
//                    ResolveSkillProficiency(result, name, value, creatureService);
//                }
//                else
//                {
//                    throw new ArgumentException($"{typeString} doesn't appear to be a valid proficiency type");
//                }

//            }
//        }
//    }

//    private static void ResolveSkillProficiency(Creature result, string skillString, int value, ICreatureService creatureService)
//    {
//        if(Enum.TryParse(typeof(CreatureSkills), skillString, true, out var skill))
//        {
//            var unproficientSkillValue = creatureService.GetSkillBonusTotal(result, (CreatureSkills)skill, CreatureSkillLevel.None);

//            if (value == unproficientSkillValue + result.ProficiencyBonus)
//            {
//                result.Proficient |= (CreatureSkills)skill;
//            }
//            else if (value == unproficientSkillValue + 2 * result.ProficiencyBonus)
//            {
//                result.Expertise |= (CreatureSkills)skill;
//            }
//            else if(value == unproficientSkillValue + result.ProficiencyBonus / 2)
//            {
//                result.HalfProficient |= (CreatureSkills)skill;
//            }
//        }
//    }

//    private static void ResolveSavingProficiency(Creature result, string saveString, int value)
//    {
//        if(saveString == "STR")
//        {
//            result.StrengthSave = value;
//        }
//        else if(saveString == "DEX")
//        {
//            result.DexteritySave = value;
//        }
//        else if (saveString == "CON")
//        {
//            result.ConstitutionSave = value;
//        }
//        else if(saveString == "WIS")
//        {
//            result.WisdomSave = value;
//        }
//        else if(saveString == "INT")
//        {
//            result.IntelligenceSave = value;
//        }
//        else if(saveString == "CHA")
//        {
//            result.CharismaSave = value;
//        }
//    }
//    private static string GetAttackDescription(ApiCreature creature)
//    {
//        if(creature.actions != null && creature.actions.Count > 0)
//        {
//            foreach(var action in creature.actions)
//            {
//                if (action.ContainsKey("name"))
//                {
//                    var name = (string)action["name"];
//                    if(name == "Multiattack")
//                    {
//                        return (string)action["desc"];
//                    }
//                }
//            }
//        }
//        return "";
//    }
//    private static void GetAbilities(Creature result, ApiCreature creature, List<Ability> spells)
//    {
//        if(creature.actions != null && creature.actions.Count > 0)
//        {
//            foreach(var action in creature.actions)
//            {
//                var name = (string)action["name"];
//                if(name != "Multiattack")
//                {
//                    var resultAbility = HandleAbility(result, action);
//                    resultAbility.ActionSpeed = ActionSpeed.Action;
//                    //something to actually add.
//                    result.Abilities.Add(resultAbility);

//                }
//            }
//        }
//        if (creature.reactions != null && creature.reactions.Count > 0)
//        {
//            foreach (var reaction in creature.reactions)
//            {
//                var resultAbility = HandleAbility(result, reaction);
//                resultAbility.ActionSpeed = ActionSpeed.Reaction;
//                //something to actually add.
//                result.Abilities.Add(resultAbility);
//            }
//        }
//        if (creature.legendary_actions != null && creature.legendary_actions.Count > 0)
//        {
//            foreach(var legendaryAction in creature.legendary_actions)
//            {
//                var resultAbility = HandleAbility(result, legendaryAction);
//                resultAbility.ActionSpeed = ActionSpeed.Legendary;
//                //something to actually add.
//                result.Abilities.Add(resultAbility);


//            }
//            //not guarantees, but safe bets
//            result.MaxLegendaryActions = 3;
//            //result.MaxLegendaryResistance = 3;
//        }


//    }

//    private static Ability HandleAbility(Creature creature, Dictionary<string, object> apiCreatureAbility)
//    {
//        Ability result = new();

//        result.Name = (string)apiCreatureAbility["name"];
//        result.EffectDescription = (string)apiCreatureAbility["desc"];

//        if (apiCreatureAbility.ContainsKey("attack_bonus"))
//        {
//            var attack_bonus_value = (long)apiCreatureAbility["attack_bonus"];
//            var bonusType = DetermineAttackScalingStat(creature, Convert.ToInt32(attack_bonus_value));

//            result.ResolutionStat = bonusType;
//            result.Resolution = ResolutionType.Attack;
//        }
//        else if (apiCreatureAbility.ContainsKey("dc"))
//        {
//            var dcDict = (dynamic)apiCreatureAbility["dc"];

//            var dcTypeDict = dcDict["dc_type"];
//            var dcValue = Convert.ToInt32((long)dcDict["dc_value"]);

//            var dcTypeString = (string)dcTypeDict["name"];

//            result.SaveType = GetCreatureAttributeFromPartialString(dcTypeString);
//            result.Resolution = ResolutionType.SavingThrow;
//        }
//        else
//        {
//            result.Resolution = ResolutionType.Undefined;
//        }

//        if (apiCreatureAbility.ContainsKey("damage"))
//        {
//            //var damageDictListObject = (List<Dictionary<string, apiCreatureAbility["damage"];

//            //TODO: crawl into dynamic reeee

//            //var damageDictList = (List<Array>)damageDictListObject;

//            var damageDictList = ((dynamic)apiCreatureAbility["damage"]).First;

//            //var dList = damageDictList["damage_type"];

//            //var damageDict = damageDictList["damage_type"];

//            if (damageDictList.ContainsKey("damage_type"))
//            {
//                var damageTypeDict = damageDictList["damage_type"];

//                var damageType = (string)damageTypeDict["name"];

//                result.DamageTypes = DamageTypeFromString(damageType);
//            }
//        }
//        result.SpellLevel = SpellLevel.NotASpell;

//        if (apiCreatureAbility.ContainsKey("usage")) //rare, but mostly is recharge or max charges information
//        {
//            var usageDict = (dynamic)apiCreatureAbility["usage"];

//            if (usageDict.ContainsKey("min_value"))
//            {
//                var refresh = (long)usageDict["min_value"];
//                if(refresh < 6)
//                {
//                    result.Name += $" (Recharge {refresh} - 6)";
//                }
//                else
//                {
//                    result.Name += $" (Recharge 6)";
//                }
//            }
//            else
//            {
//                var usageType = (string)usageDict["type"];
//                if(usageType == "recharge after rest")
//                {
//                    result.Name += " (Once per rest)";
//                }
//                else if(usageType == "per day")
//                {
//                    var times = (long)usageDict["times"];
//                    result.Name += $" ({times} charges per day)";
//                }
//            }
            
//        }

//        //other ability information has to be pulled from the descriptions... sketchy at best.

//        result.Concentration = result.EffectDescription.Contains("as if concentrating");
//        HandleAOEType(result);

//        return result;



//        //result.

//        //result.ActionSpeed = null;
//        //result.TimeDuration = null;
//        //result.Concentration = false;
        





//    }

//    private static void HandleAOEType(Ability result)
//    {
//        if (result.EffectDescription.Contains("target"))
//        {
//            result.TargetAreaType = TargetAreaType.Creatures;
//        }
//        if (result.EffectDescription.Contains("cone"))
//        {
//            result.TargetAreaType = TargetAreaType.Cone;
//        }
//        else if (result.EffectDescription.Contains("cylinder"))
//        {
//            result.TargetAreaType = TargetAreaType.Cylinder;
//        }
//    }

//    private static CreatureAttributeType GetCreatureAttributeFromPartialString(string partialString)
//    {
//        return partialString switch
//        {
//            "STR" => CreatureAttributeType.Strength,
//            "DEX" => CreatureAttributeType.Dexterity,
//            "CON" => CreatureAttributeType.Constitution,
//            "INT" => CreatureAttributeType.Intelligence,
//            "WIS" => CreatureAttributeType.Wisdom,
//            "CHA" => CreatureAttributeType.Charisma,
//            _ => throw new ArgumentException($"{partialString} is not a supported CreatureAttributeType")
//        };
//    }

//    private static DamageType DamageTypeFromString(string value)
//    {
//        if(Enum.TryParse<DamageType>(value, true, out DamageType damageType))
//        {
//            return damageType;
//        }
//        else
//        {
//            return DamageType.None;
//        }
//    }

//    private static CreatureAttributeType DetermineAttackScalingStat(Creature creature, int value)
//    {
//        var remaining = value - creature.ProficiencyBonus;

//        if (remaining == (creature.Strength - 10) / 2)
//        {
//            return CreatureAttributeType.Strength;
//        }
//        else if (remaining == (creature.Dexterity - 10) / 2)
//        {
//            return CreatureAttributeType.Dexterity;
//        }
//        else if (remaining == (creature.Constitution - 10) / 2)
//        {
//            return CreatureAttributeType.Constitution;
//        }
//        else if (remaining == (creature.Intelligence - 10) / 2)
//        {
//            return CreatureAttributeType.Intelligence;
//        }
//        else if (remaining == (creature.Wisdom - 10) / 2)
//        {
//            return CreatureAttributeType.Wisdom;
//        }
//        else if (remaining == (creature.Charisma - 10) / 2)
//        {
//            return CreatureAttributeType.Charisma;
//        }
//        return CreatureAttributeType.None;
//    }
//    private static CreatureAlignment GetAlignment(ApiCreature creature)
//    {
//        return creature.alignment switch
//        {
//            "unaligned" => CreatureAlignment.Undefined,
//            "any alignment" => CreatureAlignment.Undefined,
//            "chaotic evil" => CreatureAlignment.ChaoticEvil,
//            "chaotic good" => CreatureAlignment.ChaoticGood,
//            "chaotic neutral" => CreatureAlignment.ChaoticNeutral,
//            "lawful good" => CreatureAlignment.LawfulGood,
//            "lawful neutral" => CreatureAlignment.LawfulNeutral,
//            "lawful evil" => CreatureAlignment.LawfulEvil,
//            "neutral good" => CreatureAlignment.NeutralGood,
//            "neutral evil" => CreatureAlignment.NeutralEvil,
//            "neutral" => CreatureAlignment.TrueNeutral,
//            _ => CreatureAlignment.Undefined,
//            //_ => throw new ArgumentException($"{creature.alignment} doesn't appear to be a valid CreatureAlignment")

//        };
//    }
//    private static int GetAC(ApiCreature creature)
//    {
//        long result = 0;

//        if(creature.armor_class != null && creature.armor_class.Count > 0)
//        {
//            foreach(var ac in creature.armor_class)
//            {
//                var acValue = (long)ac["value"];

//                if (acValue > result)
//                {
//                    result = acValue;
//                }
//            }
//        }
//        return Convert.ToInt32(result);
//    }

//    public static Ability ConvertToAbility(ApiSpell apiSpell)
//    {
//        var ability = new Ability();


//        ability.ActionSpeed = GetActionSpeed(apiSpell);
//        if(ability.ActionSpeed == ActionSpeed.Other)
//        {
//            ability.CastTimeString = apiSpell.casting_time ?? "";
//        }
//        ability.Name = apiSpell.name;
//        ability.CastingComponents = GetCastingComponents(apiSpell);
//        ability.Concentration = apiSpell.concentration;
//        ability.MaterialCost = apiSpell.material ?? "";
//        ability.DamageTypes = GetDamageTypes(apiSpell);
//        ability.MagicSchool = GetMagicSchool(apiSpell);
//        ability.EffectDescription = GetDescription(apiSpell);
//        ability.Resolution = GetResolution(apiSpell);
//        ability.TargetAreaType = GetTargetAreaType(apiSpell);
//        if (ability.TargetAreaType == TargetAreaType.None && (ability.Resolution == ResolutionType.SpellAttack || ability.Resolution == ResolutionType.SavingThrow))
//        {
//            ability.TargetAreaType = TargetAreaType.Creatures;
//        }
//        ability.ResolutionStat = GetResolutionStat(apiSpell);
//        ability.SaveType = GetSaveType(apiSpell);
//        ability.SpellLevel = GetSpellLevel(apiSpell);
//        ability.TargetAreaSize = GetAoeSize(apiSpell);
//        ability.TargetCount = GetTargetCount(apiSpell);
//        ability.TargetDistance = GetTargetDistance(apiSpell);
//        ability.TargetDistanceType = GetTargetDistanceType(apiSpell);
//        ability.TimeDuration = GetTimeDuration(apiSpell);
//        ability.TimeDurationType = GetTimeDurationType(apiSpell);
        
//        ability.Id = Guid.NewGuid();

//        return ability;


//    }

//    private static TargetAreaType GetTargetAreaType(ApiSpell apiSpell)
//    {
//        if (apiSpell.area_of_effect != null && apiSpell.area_of_effect.ContainsKey("type"))
//        {
//            if(apiSpell.area_of_effect["type"] is string aoeTypestring)
//            {
//                return aoeTypestring switch
//                {
//                    "sphere" => TargetAreaType.Sphere,
//                    "cone" => TargetAreaType.Cone,
//                    "line" => TargetAreaType.Line,
//                    "cube" => TargetAreaType.Cube,
//                    "cylinder" => TargetAreaType.Cylinder,
//                    _ => throw new ArgumentException($"{aoeTypestring} is not a valid area of effect")
//                };
//            }

//        }
//        return TargetAreaType.None;   
//    }

//    private static TimeDuration GetTimeDurationType(ApiSpell apiSpell)
//    {
//        var unit = apiSpell.duration.Split(' ').Last().ToLower();
//        return unit switch
//        {
//            "instantaneous" => TimeDuration.Instantaneous,
//            "minutes" => TimeDuration.Minutes,
//            "minute" => TimeDuration.Minutes,
//            "hours" => TimeDuration.Hours,
//            "hour" => TimeDuration.Hours,
//            "round" => TimeDuration.Rounds,
//            "rounds" => TimeDuration.Rounds,
//            "day" => TimeDuration.Days,
//            "days" => TimeDuration.Days,
//            "years" => TimeDuration.Years,
//            "year" => TimeDuration.Years,
//            "dispelled" => TimeDuration.Permanent, 
//            "special" => TimeDuration.Permanent,
//            _ => throw new ArgumentException($"{unit} is not a valid TimeDuration")
//        };;
//    }
//    private static int GetTimeDuration(ApiSpell apiSpell)
//    {
//        var timeStringArray = apiSpell.duration.Split(' ');
//        if (timeStringArray.Length == 1)
//            return 0;

//        var timeString = timeStringArray[timeStringArray.Length -2];

//        if(int.TryParse(timeString, out var time))
//        {
//            return time;
//        }
//        return 0;
//    }

//    private static ActionRangeType GetTargetDistanceType(ApiSpell apiSpell)
//    {
//        var rangeString = apiSpell.range.Split(' ').Last().ToLower();

//        return rangeString switch
//        {
//            "miles" => ActionRangeType.Mile,
//            "mile" => ActionRangeType.Mile,
//            "self" => ActionRangeType.Self,
//            "feet" => ActionRangeType.Feet,
//            "touch" => ActionRangeType.Touch,
//            "special" => ActionRangeType.Unlimited,
//            "sight" => ActionRangeType.Sight,
//            "unlimited" => ActionRangeType.Unlimited,
//            _ => throw new ArgumentException($"{rangeString} is not a valid ActionRangeType")
//        };
//    }
//    private static int GetTargetCount(ApiSpell apiSpell)
//    {
//        return 1; //no clean way to pull this
//    }
//    private static int GetTargetDistance(ApiSpell apiSpell)
//    {
//        var rangeString = apiSpell.range.Split(' ').First();

//        if(int.TryParse(rangeString, out var targetDistance))
//        {
//            return targetDistance;
//        }
//        return 5;
//    }

//    private static int GetAoeSize(ApiSpell apiSpell)
//    {
//        if (apiSpell.area_of_effect != null && apiSpell.area_of_effect.ContainsKey("type"))
//        {
//            var aoeSize = (Int64)apiSpell.area_of_effect["size"];
//            return Convert.ToInt32(aoeSize);
//            //return (int)aoeSize;

//            //if (apiSpell.area_of_effect["size"] is int aoeSize)
//            //{

//            //    return aoeSize;
//            //}
//        }
//        return 0;
//    }

//    private static SpellLevel GetSpellLevel(ApiSpell apiSpell)
//    {
//        return apiSpell.level switch
//        {
//            0 => SpellLevel.Cantrip,
//            1 => SpellLevel.LevelOne,
//            2 => SpellLevel.LevelTwo,
//            3 => SpellLevel.LevelThree,
//            4 => SpellLevel.LevelFour,
//            5 => SpellLevel.LevelFive,
//            6 => SpellLevel.LevelSix,
//            7 => SpellLevel.LevelSeven,
//            8 => SpellLevel.LevelEight,
//            9 => SpellLevel.LevelNine,
//            _ => SpellLevel.NotASpell
//        };
//    }

//    private static CreatureAttributeType GetSaveType(ApiSpell apiSpell)
//    {
//        if(apiSpell.dc != null && apiSpell.dc.Keys.Count > 0)
//        {
//            var saveObj = apiSpell.dc["dc_type"];

//            var saveString = saveObj.ToString();

//            //var save = apiSpell.dc["dc_type"] as Dictionary<string, string>;

//            var saveTypeString = saveString.Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries).ToList()[2].Trim('\"');

//            return saveTypeString switch
//            {
//                "con" => CreatureAttributeType.Constitution,
//                "str" => CreatureAttributeType.Strength,
//                "dex" => CreatureAttributeType.Dexterity,
//                "wis" => CreatureAttributeType.Wisdom,
//                "int" => CreatureAttributeType.Intelligence,
//                "cha" => CreatureAttributeType.Charisma,
//                _ => throw new ArgumentException($"{saveTypeString} is not a valid save type")
//            };
//        }
//        return CreatureAttributeType.None;
//    }

//    private static CreatureAttributeType GetResolutionStat(ApiSpell apiSpell)
//    {
//        return CreatureAttributeType.None; //expect no spells to scale with a stat naturally.
//    }

//    private static ResolutionType GetResolution(ApiSpell apiSpell)
//    {
//        if (apiSpell.dc != null && apiSpell.dc.Keys.Count > 0)
//        {
//            return ResolutionType.SavingThrow;
//        }
//        else if (!String.IsNullOrEmpty(apiSpell.attack_type))
//        {
//            return ResolutionType.SpellAttack;
//        }
//        return ResolutionType.Undefined; //possible that for some spells it's willing creature, but this is an acceptable catch-all
//    }
//    private static string GetDescription(ApiSpell apiSpell)
//    {
//        var res = "";
//        foreach(var line in apiSpell.desc)
//        {
//            res += line;
//            res += "\n\n";
//        }
//        if(apiSpell.higher_level != null)
//        {
//            foreach (var line in apiSpell.higher_level)
//            {
//                res += line;
//                res += "\n\n";
//            }
//        }

//        return res;
//    }

//    private static MagicSchool GetMagicSchool(ApiSpell apiSpell)
//    {

//        if (apiSpell.school != null && apiSpell.school.Keys.Count > 0)
//        {
//            var schoolString = apiSpell.school["name"].ToLower();

//            if(Enum.TryParse(typeof(MagicSchool), schoolString, true, out var school))
//            {
//                return (MagicSchool)school;
//            }
//        }
//        return MagicSchool.None;
//    }
//    private static DamageType GetDamageTypes(ApiSpell apiSpell)
//    {
//        if(apiSpell.damage != null && apiSpell.damage.Keys.Count > 0)
//        {
//            if (!apiSpell.damage.ContainsKey("damage_type"))
//                return DamageType.Untyped;

//            var damageDict = apiSpell.damage["damage_type"] as Dictionary<string, string>;

//            var damageString = damageDict["name"];

//            if(Enum.TryParse(typeof(DamageType), damageString, true, out var damageType))
//            {
//                return (DamageType)damageType;
//            }
//        }
//        return DamageType.None;
//    }
//    private static SpellCastComponent GetCastingComponents(ApiSpell apiSpell)
//    {
//        SpellCastComponent components = SpellCastComponent.None;

//        foreach(var component in apiSpell.components)
//        {
//            if (component == "V")
//                components |= SpellCastComponent.Verbal;
//            else if (component == "S")
//                components |= SpellCastComponent.Somatic;
//            else if (component == "M")
//                components |= SpellCastComponent.Material;
//        }
//        return components;
//    }

//    public static ActionSpeed GetActionSpeed(ApiSpell api)
//    {
//        var split = api.casting_time.Split(' ');

//        return split[1] switch
//        {
//            "action" => ActionSpeed.Action,
//            "bonus" => ActionSpeed.BonusAction,
//            "reaction" => ActionSpeed.Reaction,
//            _ => ActionSpeed.Other
//        }; ;

//    }
//}
