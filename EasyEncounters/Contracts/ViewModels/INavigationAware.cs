namespace EasyEncounters.Contracts.ViewModels;

public interface INavigationAware
{
    void OnNavigatedFrom();

    void OnNavigatedTo(object parameter);
}