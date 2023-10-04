using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Services;
public class ConditionService : IConditionService
{
    public void AddCondition(ActiveEncounterCreature creature, Condition condition)
    {
        
    }

    public string GetConditionDescription(Condition condition)
    {
        //todo: move strings to resources etc.
        return GetDescriptions(condition);
       
    }

    private static string DamageTypeToString(DamageType damage)
    {
        StringBuilder sb = new();
        foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
        {
            if ((damage & damageType) != 0)
                sb.Append(damageType.ToString());
        }
        return sb.ToString();
    }

    //todo: add strings to resources
    private string GetDescriptions(Condition condition)
    {
        StringBuilder sb = new StringBuilder();

        if (condition.HasFlag(Condition.Blinded))
        {
            sb.AppendLine("A blinded creature can't see and automatically fails and ability check that requires sight. " +
                "\nAttack Rolls against the creature have advantage, " +
                "and the creature's attack rolls have disadvantage");
        }
        if (condition.HasFlag(Condition.Charmed))
        {
            sb.AppendLine("A charmed creatures can't attack the charmer of target the charmer with harmful abilities or magical effects. " +
                "\nThe charmer has advantage on any ability check to interact socially with the creature.");
        }
        if (condition.HasFlag(Condition.Deafened))
        {
            sb.AppendLine("A deafened creature can't hear and automatically fails any ability check that requires hearing.");
        }
        if (condition.HasFlag(Condition.Frightened))
        {
            sb.AppendLine("A frightened creature has disadvantage on ability checks and attack rolls wile the source of " +
                "its fear is within line of sight. \nThe creature can't willingly move closer to the source of its fear.");
        }
        if (condition.HasFlag(Condition.Incapacitated))
        {
            sb.AppendLine("An incapacitated creature can't take actions or reactions");
        }
        if (condition.HasFlag(Condition.Invisible))
        {
            sb.AppendLine("An invisible creasture is impossible to see without the aid of magic or a " +
                "special sense. For the purpose of hiding, the creature is heavily obscured. The creature's location" +
                "can be detect ed by any noise it makes or any tracks it leaves. \nAttack rolls against the creature have" +
                "disadvantage, and the creature's attack rolls have advantage.");
        }
        if (condition.HasFlag(Condition.Paralyzed))
        {
            sb.AppendLine("A paralyzed creature is incapacitated and can't move or speak.\nThe creature automatically " +
                "fails Strength and Dexterity saving throws.\nAttack rolls against the creature have advantage." +
                "\nAny attack that his the creature is a critical hit if the attacker is within 5 feet of the creature.");
        }
        if (condition.HasFlag(Condition.Petrified))
        {
            sb.AppendLine("A petrified creature is transformed, along with any nonmagical object it is wearing or " +
                "carrying, into a solid inanimate substance (usually stone). Its weight increases by a factor of ten," +
                " and it ceases aging.\nThe creature is incapacitated, can't move or speak, and is unaware of its " +
                "surroundings.\nAttack rolls against the creature have advantage.\nThe creature automatically fails" +
                " Strength and Dexterity saving throws.\nThe creature has resistance to all damage." +
                "\nThe creature is immune to poison and disease, although a poison or disease already in its " +
                "system is suspended, not neutralized.");
        }
        if (condition.HasFlag(Condition.Poisoned))
        {
            sb.AppendLine("A poisoned creature has disadvantage on attack rolls and ability checks.");
        }
        if (condition.HasFlag(Condition.Prone))
        {
            sb.AppendLine("A prone creature's only movement option is to crawl, unless it stands up and thereby" +
                "ends the condition.\nThe creature has disadvantage on attack rolls.\nAn attack roll against " +
                "the creature has advantage if the attacker is within 5 feet of the creature. " +
                "Otherwise, the attack roll has disadvantage.");
        }
        if (condition.HasFlag(Condition.Restrained))
        {
            sb.AppendLine("A restrained creature's speed becomes 0, and it can't benefit from any bonus to its " +
                "speed.\nAttack rolls against the creature have advantage, and the creature's attack rolls have" +
                " disadvantage.\nThe creature has disadvantage on Dexterity saving throws.");
        }
        if (condition.HasFlag(Condition.Stunned))
        {
            sb.AppendLine("A stunned creature is incapacitated, can't move, and can speak only falteringly. \nThe " +
                "creature automatically fails Strength and Dexterity saving throws.\nAttack rolls against the" +
                " creature have advantage.");
        }
        if (condition.HasFlag(Condition.Unconscious))
        {
            sb.AppendLine("An unconscious creature is incapacitated, can't move or speak, and is unaware of its " +
                "surroundings.\nThe creature drops whatever it's holdiing and falls prone.\nThe creature automatically" +
                " fails Strength and Dexterity saving throws.\nAttack rolls against the creature have advantage.\n" +
                "Any attack that hits the creature is a critical hit if the attacker is within 5 feet of the creature.");
        }
        if (condition.HasFlag(Condition.Exhausted))
        {
            sb.AppendLine("The first level of exhaustion causes the creature to have disadvantage on ability checks.");
        }
        if (condition.HasFlag(Condition.Exhaustion2))
        {
            sb.AppendLine("The second level of exhaustion causes the creature to have its speed halved.");
        }
        if (condition.HasFlag(Condition.Exhaustion3))
        {
            sb.AppendLine("The third level of exhaustion causes the creature to have disadvantage on " +
                "attack rolls and saving throws.");
        }
        if (condition.HasFlag(Condition.Exhaustion4))
        {
            sb.AppendLine("The fourth level of exhaustion causes the creature to have its hit point maximum " +
                "halved.");
        }
        if (condition.HasFlag(Condition.Exhaustion5))
        {
            sb.AppendLine("The fifth level of exhaustion causes the creature to have its speed reduced to 0");
        }
        if (condition.HasFlag(Condition.Exhaustion6))
        {
            sb.AppendLine("The sixth level of exhaustion causes death.");
        }

        return sb.ToString();
    }
}
