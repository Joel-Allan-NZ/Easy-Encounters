using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Persistence.ApiToModel;
public class ApiSpell
{
    public ApiSpell(string Index = "", string Name = "", List<string> Desc = null, List<string> Higher_Level = null, string Range = "", List<string> Components = null,
        string Material = "", bool Ritual = false, string Duration = "", bool Concentration = false, string Casting_Time = "", int Level = 0, string Attack_Type = "",
        Dictionary<string, Dictionary<string, string>> Damage = null, Dictionary<string, string> Damage_At_Slot_Level = null, Dictionary<string, string> School = null, List<Dictionary<string, string>> Classes = null,
        List<Dictionary<string, string>> Subclasses = null, string Url = "", Dictionary<string, object> Dc = null, Dictionary<string, object> Area_Of_Effect = null)
    {
        index = Index;
        name = Name;
        desc = Desc;
        higher_level = Higher_Level;
        range = Range;
        components = components ?? new List<string>();
        material = Material;
        ritual = Ritual;
        duration = Duration;
        concentration = Concentration;
        casting_time = Casting_Time;
        level = Level;
        attack_type = Attack_Type;
        damage = Damage ?? new Dictionary<string, Dictionary<string, string>>();
        damage_at_slot_level = Damage_At_Slot_Level ?? new();
        school = School ?? new();
        classes = Classes ?? new();
        subclasses = Subclasses ?? new();
        url = Url;
        dc = Dc ?? new Dictionary<string, object>();
        area_of_effect = Area_Of_Effect ?? new();
    }

    public ApiSpell()
    {

    }

    public string index
    {
        get; set;
    }

    public string name
    {
        get; set;
    }

    public List<string> desc
    {
        get; set;
    }

    public List<string> higher_level
    {
        get; set;
    }

    public string range
    {
        get; set;
    }

    public List<string> components
    {
        get; set;
    }

    public string material
    {
        get; set;
    }

    public bool ritual;

    public string duration;

    public bool concentration;

    public string casting_time;

    public int level;

    public string attack_type;

    public Dictionary<string, Dictionary<string, string>> damage;

    public Dictionary<string, string> damage_at_slot_level;

    public Dictionary<string, string> school;

    public List<Dictionary<string, string>> classes;

    public List<Dictionary<string, string>> subclasses;

    public string url;

    public Dictionary<string, object> dc;

    public Dictionary<string, object> area_of_effect;
}
