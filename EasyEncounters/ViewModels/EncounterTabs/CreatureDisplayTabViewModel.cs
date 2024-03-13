using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Messages;
using EasyEncounters.Models;

namespace EasyEncounters.ViewModels;

public partial class CreatureDisplayTabViewModel : ObservableRecipientTab
{
    private readonly IList<Condition> _conditions = Enum.GetValues(typeof(Condition)).Cast<Condition>().ToList();
    private readonly ICreatureService _creatureService;

    public IList<Condition> Conditions => _conditions;

    [ObservableProperty]
    private ActiveEncounterCreature? _creature;

    [ObservableProperty]
    private ObservableActiveEncounterCreature? _creatureVM;

    [ObservableProperty]
    private Uri _hyperlink;

   

    public CreatureDisplayTabViewModel(ICreatureService creatureService)
    {
        _creatureService = creatureService;
        Hyperlink = new Uri("http://www.google.com"); //placeholder

        WeakReferenceMessenger.Default.Register<UseAbilityRequestMessage>(this, (r, m) =>
        {
            if (Abilities.Contains(m.Parameter) && CreatureVM != null)
                WeakReferenceMessenger.Default.Send(new AbilityDamageRequestMessage(m.Parameter, CreatureVM));
        });
    }

    public ObservableCollection<ObservableActiveAbility> Abilities
    {
        get; private set;
    } = new();

    public override void OnTabClosed()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public override void OnTabOpened(object? parameter)
    {
        if (parameter is not null and ObservableActiveEncounterCreature creatureVM)
        {
            CreatureVM = creatureVM;
            Creature = CreatureVM.Creature;

            //parameter as ActiveEncounterCreature;

            Abilities.Clear();
            foreach (var activeAbility in Creature.ActiveAbilities)
                Abilities.Add(new ObservableActiveAbility(activeAbility));

            var bonuses = FindSkills();
            CreatureVM.HandleSkills(bonuses);            
        }
    }

    private IEnumerable<KeyValuePair<CreatureSkills, int>> FindSkills()
    {
        var relevantSkills = Enum.GetValues<CreatureSkills>().Cast<CreatureSkills>().Skip(1).ToList();

        List<KeyValuePair<CreatureSkills, int>> skillBonuses = new();

        foreach(var skill in relevantSkills)
        {
            var proficiencyLevel = _creatureService.GetSkillProficiencyLevel(Creature, skill);
            if (proficiencyLevel != CreatureSkillLevel.None)
            {
                skillBonuses.Add(new(skill, _creatureService.GetSkillBonusTotal(Creature, skill, proficiencyLevel)));
            }
        }
        return skillBonuses;
    }

    partial void OnCreatureChanged(ActiveEncounterCreature? oldValue, ActiveEncounterCreature? newValue)
    {
        if (Uri.TryCreate(newValue?.Hyperlink, UriKind.Absolute, out Uri? result))
        {
            Hyperlink = result;
        }
    }

    [RelayCommand]
    private void RequestDamage()
    {
        if (CreatureVM != null)
            WeakReferenceMessenger.Default.Send(new AbilityDamageRequestMessage(null, CreatureVM));
    }
}