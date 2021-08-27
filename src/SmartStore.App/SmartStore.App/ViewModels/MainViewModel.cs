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
            get => _menuViewModel;
            set => SetProperty(ref _menuViewModel, value);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _syncService.Sync();
            await _menuViewModel.InitializeAsync(navigationData);
            await NavigationService.NavigateToAsync<TerminalViewModel>();
        }
    }
}
