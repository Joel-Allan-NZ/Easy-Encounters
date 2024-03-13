using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models;
public class CRDifficultyGuide
{
    public List<CRDifficultyRow> Data
    {
        get; set;
    }

    public CRDifficultyGuide()
    {
        Data = new List<CRDifficultyRow>()
        {
            new("0",2,13,"1–6",3,"0–1",13),
            new("1/8",2,13,"7–35",3,"2–3",13),
            new("1/4",2,13,"36–49",3,"4–5",13),
            new("1/2",2,13,"50–70",3,"6–8",13),
            new("1",2,13,"71–85",3,"9–14",13),
            new("2",2,13,"86–100",3,"15–20",13),
            new("3",2,13,"101–115",4,"21–26",13),
            new("4",2,14,"116–130",5,"27–32",14),
            new("5",3,15,"131–145",6,"33–38",15),
            new("6",3,15,"146–160",6,"39–44",15),
            new("7",3,15,"161–175",6,"45–50",15),
            new("8",3,16,"176–190",7,"51–56",16),
            new("9",4,16,"191–205",7,"57–62",16),
            new("10",4,17,"206–220",7,"63–68",16),
            new("11",4,17,"221–235",8,"69–74",17),
            new("12",4,17,"236–250",8,"75–80",17),
            new("13",5,18,"251–265",8,"81–86",18),
            new("14",5,18,"266–280",8,"87–92",18),
            new("15",5,18,"281–295",8,"93–98",18),
            new("16",5,18,"296–310",9,"99–104",18),
            new("17",6,19,"311–325",10,"105–110",19),
            new("18",6,19,"326–340",10,"111–116",19),
            new("19",6,19,"341–355",10,"117–122",19),
            new("20",6,19,"356–400",10,"123–140",19),
            new("21",7,19,"401–445",11,"141–158",20),
            new("22",7,19,"446–490",11,"159–176",20),
            new("23",7,19,"491–535",11,"177–194",20),
            new("24",7,19,"536–580",12,"195–212",21),
            new("26",8,19,"626–670",12,"231–248",21),
            new("27",8,19,"671–715",13,"249–266",22),
            new("28",8,19,"716–760",13,"267–284",22),
            new("29",9,19,"761–805",13,"285–302",22),
            new("30",9,19,"806–850",14,"303–320",23),
        };
    }
}

public class CRDifficultyRow
{

    public CRDifficultyRow(string cR, int proficiencyBonus, int armorClass, string hp, int attackBonus, string damage, int saveDC)
    {
        CR = cR;
        ProficiencyBonus = proficiencyBonus;
        ArmorClass = armorClass;
        HP = hp;
        Damage = damage;
        AttackBonus = attackBonus;
        SaveDC = saveDC;
    }

    public string CR
    {
        get; set;
    }
    public int ProficiencyBonus
    {
        get; set;
    }
    public int ArmorClass
    {
        get; set;
    }
    public string HP
    {
        get; set;
    }
    public int AttackBonus
    {
        get; set;
    }
    public string Damage
    {
        get; set;
    }

    public int SaveDC
    {
        get; set;
    }
}
