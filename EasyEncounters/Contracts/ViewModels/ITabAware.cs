namespace EasyEncounters.Contracts.ViewModels;

public interface ITabAware
{
    void OnTabClosed();

    void OnTabOpened(object parameter);
}