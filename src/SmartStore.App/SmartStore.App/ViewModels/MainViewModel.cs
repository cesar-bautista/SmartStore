using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.ViewModels.Base;

namespace SmartStore.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        private MenuViewModel _menuViewModel;
        private HomeViewModel _homeViewModel;

        public MainViewModel(INavigationService navigationService, MenuViewModel menuViewModel, HomeViewModel homeViewModel)
        {
            _navigationService = navigationService;
            _menuViewModel = menuViewModel;
            _homeViewModel = homeViewModel;
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

        public HomeViewModel HomeViewModel
        {
            get
            {
                return _homeViewModel;
            }

            set
            {
                _homeViewModel = value;
                OnPropertyChanged();
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            return Task.WhenAll
            (
                _menuViewModel.InitializeAsync(navigationData),
                _navigationService.NavigateToAsync<HomeViewModel>()
            );
        }
    }
}
