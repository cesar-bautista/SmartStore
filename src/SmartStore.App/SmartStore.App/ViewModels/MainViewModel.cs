using System.Threading.Tasks;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.ViewModels.Base;
using SmartStore.App.ViewModels.Terminal;

namespace SmartStore.App.ViewModels
{
    public sealed class MainViewModel : BaseViewModel
    {
        private MenuViewModel _menuViewModel;
        private readonly ISyncService _syncService;

        public MainViewModel(MenuViewModel menuViewModel, ISyncService syncService)
        {
            _menuViewModel = menuViewModel;
            _syncService = syncService;
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
                _syncService.Initialize(),
                _menuViewModel.InitializeAsync(navigationData),
                NavigationService.NavigateToAsync<TerminalViewModel>()
            );
        }
    }
}
