using System.Threading.Tasks;
using SmartStore.App.ViewModels.Base;
using SmartStore.App.ViewModels.Terminal;

namespace SmartStore.App.ViewModels
{
    public sealed class MainViewModel : BaseViewModel
    {
        private MenuViewModel _menuViewModel;

        public MainViewModel(MenuViewModel menuViewModel)
        {
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
                NavigationService.NavigateToAsync<TerminalViewModel>()
            );
        }
    }
}
