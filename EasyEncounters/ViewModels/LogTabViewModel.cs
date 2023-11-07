using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Messages;
using EasyEncounters.Models;

namespace EasyEncounters.ViewModels;
public class LogTabViewModel : ObservableRecipientTab
{
    public ObservableCollection<string> CombatLog
    {
        get; private set;
    } = new();

    public override void OnTabClosed()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    public override void OnTabOpened(object parameter)
    {
        WeakReferenceMessenger.Default.Register<LogMessageLogged>(this, (r, m) =>
        {
            DamageLogged(m.LogMessages);
        });
    }


    private void DamageLogged(IList<string> toLog)
    {
        foreach (var msg in toLog)
            CombatLog.Add(msg);
    }
}
