using System;
using System.Threading.Tasks;
using SmartStore.App.ViewModels.Base;

namespace SmartStore.App.Abstractions.Ui
{
    public interface INavigationService
    {
        Task InitializeAsync();

        Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;

        Task NavigateToAsync(Type viewModelType, object parameter = null);

        Task NavigateBackAsync(bool isRefresh = false);

        Task RemoveLastFromBackStackAsync();
    }
}
