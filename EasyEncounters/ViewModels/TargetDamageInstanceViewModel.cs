using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Messages;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.ViewModels;
public partial class TargetDamageInstanceViewModel : ObservableRecipient
{

    [ObservableProperty]
    private DamageType _selectedDamageType;

    [ObservableProperty]
    private int _damageAmount;

    partial void OnSelectedDamageTypeChanged(DamageType value)
    {
        WeakReferenceMessenger.Default.Send(new DamageTypeChangedMessage(value));
    }

    public ObservableCollection<DamageCreatureViewModel> Targets
    {
        get; private set;
    } = new();

    /// <summary>
    /// For naming damage instances uniquely, quickly.
    /// </summary>
    public int Count
    {
        get; set;
    }


    public string Name
    {
        get; set;
    }

    public void AddTarget(DamageCreatureViewModel target)
    {
        if (!Targets.Contains(target, new DamageCreatureViewModelComparer()))
            Targets.Add(target);
    }

    public void RemoveTarget(DamageCreatureViewModel target)
    {
        if (Targets.Contains(target, new DamageCreatureViewModelComparer()))
            Targets.Remove(target);
    }

    [RelayCommand]
    private void CopyDamageRequest()
    {
        WeakReferenceMessenger.Default.Send(new TargetedDamageCopyRequestMessage(this));
    }

    [RelayCommand]
    private void DeleteDamageRequest()
    {
        WeakReferenceMessenger.Default.Send(new TargetedDamageDeleteRequestMessage(this));
    }



    public TargetDamageInstanceViewModel(int count)
    {
        Count = count;

        Name = $"Damage {Count}";

    }
}

internal class DamageCreatureViewModelComparer : IEqualityComparer<DamageCreatureViewModel>
{
    public bool Equals(DamageCreatureViewModel? x, DamageCreatureViewModel? y)
    {
        if (x != null && x.ActiveEncounterCreatureViewModel != null && y != null && y.ActiveEncounterCreatureViewModel != null)
        {
            return x.ActiveEncounterCreatureViewModel.Creature.EncounterID == x.ActiveEncounterCreatureViewModel.Creature.EncounterID;
        }
        return false;
    }
    public int GetHashCode([DisallowNull] DamageCreatureViewModel obj)
    {
        if (obj.ActiveEncounterCreatureViewModel != null)
            return obj.ActiveEncounterCreatureViewModel.Creature.EncounterID.GetHashCode();
        else
            throw new ArgumentNullException(nameof(obj), "This ActiveEncounterCreatureViewModel cannot be null"); //lazy cover. todo: improve

    }
}
