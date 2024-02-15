using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.ViewModels;
public partial class ConditionTypesViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _enumString;

    [ObservableProperty]
    private string _blindTooltip = "- Can't see and fails ability checks that require sight.\n- Attack Rolls have disadvantage, and attack rolls against the creature have advantage.";

    [ObservableProperty]
    private string _charmTooltip = "- A charmed creature can't attack the charmer or target the charmer with harmful abilities or magical effects.\n- The charmer has advantage " +
        "on any ability check to interact socially with the creature.";

    [ObservableProperty]
    private string _deafTooltip = "- A deafened creature can't hear, and automatically fails any ability check that requires hearing.";

    [ObservableProperty]
    private string _exhaustionTooltip = "- Level 1: Disadvantage on ability checks\n- Level 2: Speed halved.\n- Level 3: Disadvantage on attack rolls and saves. " +
        "\n- Level 4: HP maximum halved.\n- Level 5: Speed reduced to 0.\n- Level 6: Death";

    [ObservableProperty]
    private string _frightenTooltip = "- A frightened creature has disadvantage on ability checks and attack rolls while the source of fear is within line of sight. " +
        "\n- The creature can't willingly move closer to the source of its fear.";

    [ObservableProperty]
    private string _grappledTooltip = "- A grappled creature's movement speed becomes 0.\n- The condition ends if the grappler is Incapacitated\n" +
        "-The condition also ends if an effect removes the grappled creature from the reach of the grappler or grappling effect.";

    [ObservableProperty]
    private string _incapacitatedTooltip = "- An incapacitated creature can't take actions or reactions";

    [ObservableProperty]
    private string _paralyzedTooltip = "- A paralyzed creature is incapacitated, and can't move or speak.\n- The creature fails " +
        "strength and dexterity saving throws.\n- Attack rolls against the creature have advantage.\n- Any attack from within 5 ft" +
        " that hits the creature is a critical hit.";

    [ObservableProperty]
    private string _petrifiedTooltip = "- A petrified creature's weight increases by a factor of 10, and it ceases aging.\n- The creature" +
        " is incapacitated, can't move or speak, and is unaware of its surroundings.\n- Attack rolls against the creature have " +
        "advantage.\n- The creature fails Strength and Dexterity saves.\n- The creature has resistsance to all damage.\n- The creature " +
        "is immune to poison and disease, but an active poison or disease is merely suspended, not cured.";

    [ObservableProperty]
    private string _poisonedTooltip = "- A poisoned creature has disadvantage on attack rolls and ability checks.";

    [ObservableProperty]
    private string _proneTooltip = "-A prone creature's only movement option is to crawl, unless it stands up and ends the condition.\n- " +
        "The creature has disadvantage on attack rolls.\n- Attack rolls from within 5 feet have advantage. Other attack rolls have disadvantage";

    [ObservableProperty]
    private string _restrainedTooltip = "- A restrained creature's movement is 0.\n- Attack rolls have advantage, and the creature's have disadvantage.\n" +
        "- Disadvantage on Dexterity saves.";

    [ObservableProperty]
    private string _stunnedTooltip = "- A stunned creature is Incapacitated, can't move, and only speak falteringly.\n- " +
        "The creature fails Strength and Dexterity saving throws.\n- Attack rolls have advantage.";

    [ObservableProperty]
    private string _unconsciousTooltip = "- An unconscious creature is Incapacitated, can't move or speak, and is unaware of its surroundings.\n- " +
        "The creature drops whatever it was holding and falls prone.\n- The creature fails Strength and Dexterity saves.\n- Attack rolls have " +
        "advantage.\n- Attacks from within 5 ft that hit are critical strikes.";

    public Condition ConditionTypes
    {
        get; set;
    }

    public bool Blinded
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Charmed
    {
        get => Flagged();
        set => HandleFlag(value);
    }
    public bool Deafened
    {
        get => Flagged();
        set => HandleFlag(value);
    }
    public bool Frightened
    {
        get => Flagged();
        set => HandleFlag(value);
    }
    public bool Grappled
    {
        get => Flagged();
        set => HandleFlag(value);
    }
    public bool Incapacitated
    {
        get => Flagged();
        set => HandleFlag(value);
    }
    public bool Paralyzed
    {
        get => Flagged();
        set => HandleFlag(value);
    }
    public bool Petrified
    {
        get => Flagged();
        set => HandleFlag(value);
    }
    public bool Poisoned
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Prone
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Restrained
    {
        get => Flagged();
        set => HandleFlag(value);
    }
    public bool Stunned
    {
        get => Flagged();
        set => HandleFlag(value);
    }
    public bool Unconscious
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Exhausted
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public ConditionTypesViewModel(Condition condition)
    {
        ConditionTypes = condition;
        EnumString = ConditionTypes.ToString();
    }

    private void HandleFlag(bool value, [CallerMemberName] string? name = null)
    {
        if (name != null)
        {
            var flagged = Flagged(name);
            if (flagged != value)
            {

                if (flagged)
                    RemoveFlag(name);
                else
                    AddFlag(name);
            }
            EnumString = ConditionTypes.ToString();
        }
    }

    private bool Flagged([CallerMemberName] string? name = null)
    {
        if (name != null)
            return ConditionTypes.HasFlag((Condition)Enum.Parse(typeof(Condition), name));
        else
            throw new ArgumentException("Null is a not a valid Flag Property Name");
    }

    private void AddFlag(string name)
    {
        ConditionTypes |= (Condition)Enum.Parse(typeof(Condition), name);
    }

    private void RemoveFlag(string name)
    {
        ConditionTypes &= ~(Condition)Enum.Parse(typeof(Condition), name);
    }
}
