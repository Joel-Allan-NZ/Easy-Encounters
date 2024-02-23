using EasyEncounters.Contracts.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.Models;

public class Tab : ITab
{
    public Tab(string name, Page content)
    {
        Content = content;
        Name = name;
    }

    public Page Content
    {
        get;
    }

    public string Name
    {
        get;
    }
}