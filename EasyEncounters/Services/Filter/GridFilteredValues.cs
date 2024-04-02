using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace EasyEncounters.Services.Filter;

public abstract partial class GridFilteredValues : ObservableObject
{
    protected bool _sortAscending;
    private readonly int _pageSize = 50;
    protected string? _sortTag;
    protected List<string> _namesCache;

    [ObservableProperty]
    private string _searchString;

    public abstract Task ResetAsync();

    public RelayCommand ResetFilterCommand
    {
        get => new(async () => await ResetFilter(), () => true);
    }
    private async Task ResetFilter()
    {
        await ResetAsync();
    }
    protected void HandleDataGrid(DataGridColumnEventArgs? e, string safeTag)
    {
        if (e != null)
        {
            _sortAscending = e.Column.SortDirection == DataGridSortDirection.Ascending;

            _sortTag = e.Column.Tag?.ToString() ?? safeTag; //should never happen, but safe catch

            if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            {
                e.Column.SortDirection = DataGridSortDirection.Ascending;
            }
            else
            {
                e.Column.SortDirection = DataGridSortDirection.Descending;
            }
        }
    }
    public ObservableCollection<string> Names
    {
        get; set;
    } = new();

    [RelayCommand(CanExecute = nameof(CanFirstAsync))]
    private async void FirstAsync() => await GetPaginatedList(1, _pageSize);

    [RelayCommand(CanExecute = nameof(CanPreviousAsync))]
    private async void PreviousAsync() => await GetPaginatedList(PageNumber - 1, _pageSize);

    [RelayCommand(CanExecute = nameof(CanNextAsync))]
    private async void NextAsync() => await GetPaginatedList(PageNumber + 1, _pageSize);

    [RelayCommand(CanExecute = nameof(CanLastAsync))]
    private async void LastAsync() => await GetPaginatedList(PageCount, _pageSize);

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(FirstAsyncCommand))]
    [NotifyCanExecuteChangedFor(nameof(PreviousAsyncCommand))]
    [NotifyCanExecuteChangedFor(nameof(NextAsyncCommand))]
    [NotifyCanExecuteChangedFor(nameof(LastAsyncCommand))]
    private int _pageCount;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(FirstAsyncCommand))]
    [NotifyCanExecuteChangedFor(nameof(PreviousAsyncCommand))]
    [NotifyCanExecuteChangedFor(nameof(NextAsyncCommand))]
    [NotifyCanExecuteChangedFor(nameof(LastAsyncCommand))]
    private int _pageNumber;


    private bool CanFirstAsync() => PageNumber != 1;
    private bool CanPreviousAsync() => PageNumber > 1;
    private bool CanNextAsync() => PageNumber < PageCount;
    private bool CanLastAsync() => PageNumber != PageCount;


    [RelayCommand]
    private async Task SearchTextChange(AutoSuggestBoxTextChangedEventArgs e)
    {
        if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (_namesCache == null)
                return;

            Names.Clear();

            if (String.IsNullOrEmpty(SearchString))
            {
                foreach (var name in _namesCache)
                {
                    Names.Add(name);
                }
                await GetPaginatedList(1, _pageSize);

            }
            else
            {
                foreach (var name in _namesCache)
                {
                    if (name.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Names.Add(name);
                    }
                }
            }


        }
    }

    [RelayCommand]
    private void SuggestionChosen(AutoSuggestBoxSuggestionChosenEventArgs e)
    {
        SearchString = (string)e.SelectedItem;
    }

    [RelayCommand]
    private async Task DataGridSort(DataGridColumnEventArgs e) => await GetPaginatedList(1, _pageSize, e);

    [RelayCommand]
    private async Task ApplyFilter() => await GetPaginatedList(1, _pageSize);

    public abstract Task GetPaginatedList(int pageIndex, int pageSize, DataGridColumnEventArgs? e = null);

    public async Task RefreshAsync() => await GetPaginatedList(1, _pageSize);
}
/// <summary>
/// Base class for Filtering a type, allows IOC for the DataService as well as providing relevant sorting and filtering properties.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract partial class GridFilteredValues<T> : GridFilteredValues, IFilterValues<T>
{




    public abstract IQueryable<T> FilterAndSortQuery<U>(IQueryable<T> queryable, U? additionalData, DataGridColumnEventArgs? e = null) where U : class;


    protected void SortByPredicate<U>(ICollection<T> collection, Func<T, U> expression, bool ascending)
    {
        IEnumerable<T> tmp = (ascending) ? collection.OrderBy(expression).ToList() : collection.OrderByDescending(expression).ToList();

        collection.Clear();
        foreach (var item in tmp)
            collection.Add(item);
    }

    protected IOrderedQueryable<T> SortQueryableByPredicate<U>(IQueryable<T> queryable, Expression<Func<T, U>> expression, bool ascending)
    {
        return ascending ? queryable.OrderBy(expression) : queryable.OrderByDescending(expression);
    }

    






}