using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.ViewModels.Base;
using SmartStore.App.ViewModels.Terminal;

namespace SmartStore.App.ViewModels
{
    public sealed class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        private MenuViewModel _menuViewModel;

        public MainViewModel(INavigationService navigationService, MenuViewModel menuViewModel)
        {
            _navigationService = navigationService;
            _menuViewModel = menuViewModel;
        }

        public MenuViewModel MenuViewModel
        {
            get
            {
                return _menuViewModel;
            }

            set
            {
                _menuViewModel = value;
                OnPropertyChanged();
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            return Task.WhenAll
            (
                _menuViewModel.InitializeAsync(navigationData),
                _navigationService.NavigateToAsync<TerminalViewModel>()
            );
        }
    }
}
