using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;

namespace EasyEncounters.Models;

/// <summary>
/// An observable wrapper for ActiveAbility
/// </summary>
public partial class ObservableActiveAbility : ObservableAbility
{
    private readonly ActiveAbility _ability;

    public ObservableActiveAbility(ActiveAbility activeAbility) : base(activeAbility)
    {
        _ability = activeAbility;

        IsSpell = (activeAbility.SpellLevel != Core.Models.Enums.SpellLevel.NotASpell);

        //if (activeAbility.SpellLevel != Core.Models.Enums.SpellLevel.NotASpell)
        //{
        //    IsSpell = Visibility.Visible;

        //}
        //else
        //    IsSpell = Visibility.Collapsed;
    }

    public int ResolutionValue
    {
        get => _ability.ResolutionValue;
        set => SetProperty(_ability.ResolutionValue, value, _ability, (m, v) => m.ResolutionValue = v);
    }

    ////todo: switch to bool and use IValueConverter
    //[ObservableProperty]
    //private Visibility _isSpell;

    [RelayCommand]
    private void UseAbilityRequested()
    {
        //todo: fire off message
        WeakReferenceMessenger.Default.Send(new UseAbilityRequestMessage(this));
    }
}