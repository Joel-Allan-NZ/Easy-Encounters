using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.Contracts.ViewModels;

public interface ITab
{
    Page Content
    {
        get;
    }

    string Name
    {
        get;
    }
}