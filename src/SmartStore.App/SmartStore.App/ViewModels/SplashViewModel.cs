using System.Threading.Tasks;
using SmartStore.App.Abstractions.Device;
using SmartStore.App.ViewModels.Base;

namespace SmartStore.App.ViewModels
{
    public sealed class SplashViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;

        public SplashViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (string.IsNullOrWhiteSpace(_settingsService.AuthAccessToken))
                await NavigationService.NavigateToAsync<LoginViewModel>();
            else
                await NavigationService.NavigateToAsync<MainViewModel>();
        }
    }
}
