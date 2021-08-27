using System.Threading.Tasks;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Abstractions.Device;
using SmartStore.App.ViewModels.Base;

namespace SmartStore.App.ViewModels
{
    public sealed class SplashViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly ISyncService _syncService;

        public SplashViewModel(ISettingsService settingsService, ISyncService syncService)
        {
            _settingsService = settingsService;
            _syncService = syncService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _syncService.Initialize();

            if (string.IsNullOrWhiteSpace(_settingsService.AuthAccessToken))
                await NavigationService.NavigateToAsync<LoginViewModel>();
            else
                await NavigationService.NavigateToAsync<MainViewModel>();
        }
    }
}
