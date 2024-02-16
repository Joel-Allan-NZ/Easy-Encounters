using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Models;

namespace EasyEncounters.Contracts.Services;

public interface ITabService
{
    void CloseTab(ObservableRecipientTab tab);
    ObservableRecipientTab OpenTab(string tabKey, object? parameter, string? name = null, bool closeable = true);
}
