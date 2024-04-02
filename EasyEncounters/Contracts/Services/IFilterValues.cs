using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Input;

namespace EasyEncounters.Contracts.Services;

public interface IFilterValues<T>
{
    public IQueryable<T> FilterAndSortQuery<U>(IQueryable<T> queryable, U? additionalData, DataGridColumnEventArgs? e = null) where U : class;

    public RelayCommand ResetFilterCommand { get; }

    public abstract Task ResetAsync();

}
