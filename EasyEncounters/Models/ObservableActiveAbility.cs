using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Helpers;
using EasyEncounters.Messages;
using Microsoft.UI.Xaml;
using Windows.System;

namespace EasyEncounters.Models;
/// <summary>
/// An observable wrapper for ActiveAbility
/// </summary>
public partial class ObservableActiveAbility : ObservableAbility
{
    private readonly ActiveAbility _ability;

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
    
}
