using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Views;
using Microsoft.UI.Xaml.Media.Animation;

namespace EasyEncounters.ViewModels
{
    public partial class EncounterTabViewModel : ObservableRecipient, INavigationAware
    {
        private readonly ITabService _tabService;
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IActiveEncounterService _activeEncounterService;

        private ActiveEncounter _activeEncounter;

        [ObservableProperty]
        private ObservableRecipientTab _selectedTab;

        public ObservableCollection<string> CombatLog
        {

            get; set;
        } = new();

        public ObservableCollection<ActiveEncounterCreatureViewModel> Creatures
        {
            get; private set;
        } = new();

        public ObservableCollection<ObservableRecipientTab> Tabs
        {
            get; private set;
        } = new();

        [RelayCommand]
        private async void RollInitiative()
        {
            var orderedInitiative = await _activeEncounterService.UpdateInitiativeOrder(_activeEncounter);

            var tempAECreatureList = new List<ActiveEncounterCreatureViewModel>(Creatures);
            Creatures.Clear();

            foreach (var orderedCreature in orderedInitiative)
            {
                foreach (var creature in tempAECreatureList)
                {
                    if (creature.IsWrapperFor(orderedCreature))
                    {
                        var ph = creature.Creature.Initiative;
                        creature.Creature.Initiative = -5;
                        creature.Creature.Initiative = ph; //todo: improve this hacky fix
                        Creatures.Add(creature);

                        break;
                    }
                }
            }

            await _dataService.SaveAddAsync(_activeEncounter);
        }

        [RelayCommand]
        private async void NextTurn()
        {
            await _activeEncounterService.EndCurrentTurnAsync(_activeEncounter);

            if (_activeEncounter.CreatureTurns.Count != Creatures.Count)
            {
                var temp = new List<ActiveEncounterCreatureViewModel>(Creatures);
                Creatures.Clear();
                foreach (var creature in temp)
                {
                    foreach (var creatureTurn in _activeEncounter.CreatureTurns)
                    {
                        if (creature.IsWrapperFor(creatureTurn))
                        {
                            if (creatureTurn == _activeEncounter.ActiveTurn)
                            {
                                creature.Targeted = true;
                                ShowCreatureDisplayTab(creature);
                            }
                            else
                                creature.Targeted = false;

                            Creatures.Add(creature);
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var creature in Creatures)
                {
                    if (_activeEncounter.ActiveTurn.Equals(creature.Creature))
                    {                   
                        creature.Targeted = true;
                        ShowCreatureDisplayTab(creature);
                    }
                    else
                        creature.Targeted = false;
                }
            }

            WeakReferenceMessenger.Default.Send(new EncounterCreatureChangedMessage(Creatures));
        }

        [RelayCommand]
        private async void EndEncounter()
        {
            await _activeEncounterService.EndEncounterAsync(_activeEncounter);
            foreach(var tab in Tabs)
            {
                tab.OnTabClosed();
            }
            if (_navigationService.CanGoBack)
                _navigationService.GoBack();
            else
                _navigationService.NavigateTo(typeof(CampaignSplashViewModel).FullName!, clearNavigation: true);
        }

        [RelayCommand]
        private void CloseTab(object obj)
        {
            if (obj != null && obj is ObservableRecipientTab)
            {
                var tab = (ObservableRecipientTab)obj;
                Tabs.Remove(tab);
                _tabService.CloseTab(tab);
            }
        }

        [RelayCommand]
        private void ShowCreatureDisplayTab(ActiveEncounterCreatureViewModel obj)
        {
            if (obj != null && obj is ActiveEncounterCreatureViewModel)
            {
                //var aecVM = obj as ActiveEncounterCreatureViewModel;

                var openTab = Tabs.FirstOrDefault(x => x.TabName == obj.Creature.EncounterName);

                if (openTab == null)
                {
                    openTab = _tabService.OpenTab(typeof(CreatureDisplayTabViewModel).FullName!,obj, obj.Creature.EncounterName);
                    Tabs.Add(openTab);
                }
                
                SelectedTab = openTab;
            }
        }

        private void ShowDamageTab(ObservableActiveAbility abilityVM, ActiveEncounterCreatureViewModel source)
        {
            var openTab = Tabs.FirstOrDefault(x => x.TabName == $"Damage from {source.Creature.EncounterName}");
            if (openTab == null)
            {
                if (abilityVM != null)
                    openTab = _tabService.OpenTab(typeof(EncounterDamageTabViewModel).FullName!,
                        new EncounterDamageTabData(_activeEncounter, source, Creatures, abilityVM), $"Damage from {source.Creature.EncounterName}");
                else
                    openTab = _tabService.OpenTab(typeof(EncounterDamageTabViewModel).FullName!,
                        new EncounterDamageTabData(_activeEncounter, source, Creatures, null), $"Damage from {source.Creature.EncounterName}");
                        //Tuple.Create(_activeEncounter, source), $"Damage from {source.Creature.EncounterName}");

                Tabs.Add(openTab);
            }
            else
            {
                ((EncounterDamageTabViewModel)openTab).SelectedAbility = abilityVM;
                ((EncounterDamageTabViewModel)openTab).SelectedDamageType = abilityVM.DamageType;
            }
            SelectedTab = openTab;


            //if (abilityVM != null)
            //    Tabs.Add(_tabService.OpenTab(typeof(EncounterDamageTabViewModel).FullName!, Tuple.Create(_activeEncounter, source, abilityVM), $"Damage from {source.Creature.EncounterName}"));
            //else
            //    Tabs.Add(_tabService.OpenTab(typeof(EncounterDamageTabViewModel).FullName!, Tuple.Create(_activeEncounter, source), $"Damage from {source.Creature.EncounterName}"));

        }


        public EncounterTabViewModel(ITabService tabService, IActiveEncounterService activeEncounterService, 
            INavigationService navigationService, IDataService dataService)
        {
            _tabService = tabService;
            _activeEncounterService = activeEncounterService;
            _navigationService = navigationService;
            _activeEncounterService = activeEncounterService;
            _dataService = dataService;

            WeakReferenceMessenger.Default.Register<AbilityDamageRequestMessage>(this, (r, m) =>
            {
                ShowDamageTab(m.Ability, m.SourceCreature);
            });

            WeakReferenceMessenger.Default.Register<DamageSourceSelectedMessage>(this, (r, m) =>
            {
                ShowDamageTab(null, m.Value);
            });

            WeakReferenceMessenger.Default.Register<InspectRequestMessage>(this, (r, m) =>
            {
                ShowCreatureDisplayTab(m.Parameter);
            });
            WeakReferenceMessenger.Default.Register<LogMessageLogged>(this, (r, m) =>
            {
                DamageLogged(m.LogMessages);
            });
        }

        //private void DealDamage(DamageInstance damage)
        //{
        //    var dmg = _activeEncounterService.DealDamage(_activeEncounter, damage);
        //    //CombatLog.Insert(0, _activeEncounter.Log.Last());
        //    WeakReferenceMessenger.Default.Send(new LogMessageLogged(dmg));

        //}
        private void DamageLogged(IList<string> toLog)
        {
            foreach (var msg in toLog.Reverse())
                CombatLog.Add(msg);
        }



        public void OnNavigatedFrom()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        public async void OnNavigatedTo(object parameter)
        {
            if(parameter != null && parameter is ActiveEncounter)
            {
                _activeEncounter = (ActiveEncounter)parameter;
            }
            else
            {
                _activeEncounter = await _dataService.GetActiveEncounterAsync();
            }
            Creatures.Clear();

            foreach (var creature in _activeEncounter.CreatureTurns)
                Creatures.Add(new ActiveEncounterCreatureViewModel(creature));

            foreach (var combatLogString in _activeEncounter.Log.Reverse<string>())
                CombatLog.Add(combatLogString);
        }
    }
}
