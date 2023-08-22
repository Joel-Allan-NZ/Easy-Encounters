using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using Microsoft.UI.Xaml;

namespace EasyEncounters.ViewModels;
public partial class ActiveEncounterCreatureViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ActiveEncounterCreature creature;

    [ObservableProperty]
    private int currentHP;

    [ObservableProperty]
    private Thickness targetVisibility;

    [ObservableProperty]
    private bool targeted;

    partial void OnTargetedChanged(bool value)
    {
        if (value)
        {
            TargetVisibility = new Thickness(1);
        }
        else
            TargetVisibility = new Thickness(0);
    }

    public string Notes
    {
        get => Creature.Notes;
        set
        {
            Creature.Notes = value;
            OnPropertyChanged();
        }
    }

    public bool Dead
    {
        get => Creature.Dead;
        set
        {
            if(Creature.Dead != value)
            {
                Creature.Dead = value;
                OnPropertyChanged();
            }
        }
    }

    public bool Reaction
    {
        get => Creature.Reaction;
        set
        {
            if (Creature.Reaction != value)
            {
                Creature.Reaction = value;
                OnPropertyChanged();
            }
        }
    }

    [RelayCommand]
    private void ToggleTarget()
    {
        Targeted = !Targeted;
        //if (TargetVisibility.Bottom == 0)
        //{
        //    TargetVisibility = new Microsoft.UI.Xaml.Thickness(1);
        //    Targeted = true;
        //}
        //else
        //{
        //    TargetVisibility = new Microsoft.UI.Xaml.Thickness(0);
        //    Targeted = false;
        //}
    }

    [RelayCommand]
    private void DamageRequested()
    {
        WeakReferenceMessenger.Default.Send(new DamageSourceSelectedMessage(this));
    }



    public ActiveEncounterCreatureViewModel(ActiveEncounterCreature creature)
    {
        Creature = creature;
        TargetVisibility = new Thickness(0);
        CurrentHP = creature.CurrentHP;
    }




}
