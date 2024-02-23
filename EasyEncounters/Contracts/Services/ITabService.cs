using EasyEncounters.Models;

namespace EasyEncounters.Contracts.Services;

public interface ITabService
{
    void CloseTab(ObservableRecipientTab tab);

    ObservableRecipientTab OpenTab(string tabKey, object? parameter, string? name = null, bool closeable = true);
}