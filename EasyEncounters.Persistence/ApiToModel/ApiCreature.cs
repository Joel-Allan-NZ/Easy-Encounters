using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Persistence.ApiToModel;
public class ApiCreature
{
    public string index;
    public string name;
    public string size;
    public string type;
    public string subtype;
    public string alignment;
    public List<Dictionary<string, object>> armor_class;
    public int hit_points;
    public string hit_dice;
    public string hit_points_roll;
    public Dictionary<string, string> speed;
    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;

    public List<Dictionary<string, object>> proficiencies;

    public List<string> damage_vulnerabilities;
    public List<string> damage_resistances;
    public List<string> damage_immunities;
    public List<Dictionary<string, string>> condition_immunities;
    public Dictionary<string, object> senses;
    public string languages;
    public double challenge_rating;
    public int proficiency_bonus;
    public int xp;
    public List<Dictionary<string, object>> special_abilities; //features

    public List<Dictionary<string, object>> actions;

    public List<Dictionary<string, object>> legendary_actions;
    public List<Dictionary<string, object>> reactions;

    public string image;
    public string url;

    public ApiCreature()
    {
        
    }

    public ApiCreature(string index, string name, string size, string type, string subtype, string alignment, List<Dictionary<string, object>> armor_class, int hit_points, string hit_dice, string hit_points_roll, Dictionary<string, string> speed, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma, List<Dictionary<string, object>> proficiencies, List<string> damage_vulnerabilities, List<string> damage_resistances, List<string> damage_immunities, List<Dictionary<string, string>> condition_immunities, Dictionary<string, object> senses, string languages, int challenge_rating, int proficiency_bonus, int xp, List<Dictionary<string, object>> special_abilities, List<Dictionary<string, object>> actions, List<Dictionary<string, object>> legendary_actions, List<Dictionary<string, object>> reactions, string image, string url)
    {
        this.index = index ?? "";
        this.name = name ?? "new creature";
        this.size = size ?? "";
        this.type = type ?? "";
        this.subtype = subtype ?? "";
        this.alignment = alignment ?? "";
        this.armor_class = armor_class ?? null;
        this.hit_points = hit_points;
        this.hit_dice = hit_dice ?? "";
        this.hit_points_roll = hit_points_roll ?? "";
        this.speed = speed ?? null;
        this.strength = strength;
        this.dexterity = dexterity;
        this.constitution = constitution;
        this.intelligence = intelligence;
        this.wisdom = wisdom;
        this.charisma = charisma;
        this.proficiencies = proficiencies;
        this.damage_vulnerabilities = damage_vulnerabilities;
        this.damage_resistances = damage_resistances;
        this.damage_immunities = damage_immunities ;
        this.condition_immunities = condition_immunities;
        this.senses = senses;
        this.languages = languages ?? "";
        this.challenge_rating = challenge_rating;
        this.proficiency_bonus = proficiency_bonus;
        this.xp = xp;
        this.special_abilities = special_abilities;
        this.actions = actions;
        this.legendary_actions = legendary_actions;
        this.reactions = reactions;
        this.image = image ?? "";
        this.url = url ?? "";
    }
}
