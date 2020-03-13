using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.ViewModels.Base;

namespace SmartStore.App.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;

        public SplashViewModel(INavigationService navigationService, ISettingsService settingsService)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (string.IsNullOrWhiteSpace(_settingsService.AuthAccessToken))
                await _navigationService.NavigateToAsync<LoginViewModel>();
            else
                await _navigationService.NavigateToAsync<MainViewModel>();
        }
    }
}
