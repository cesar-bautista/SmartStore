using System;
using System.Threading.Tasks;
using SmartStore.App.ViewModels.Base;

namespace SmartStore.App.Abstractions
{
    public interface INavigationService
    {
        Task InitializeAsync();

        Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;

        Task NavigateToAsync(Type viewModelType, object parameter = null);

        Task NavigateBackAsync();

        Task RemoveLastFromBackStackAsync();
    }
}
