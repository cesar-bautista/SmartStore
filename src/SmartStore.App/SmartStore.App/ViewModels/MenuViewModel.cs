using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels
{
    public sealed class MenuViewModel : BaseViewModel
    {
        private ObservableCollection<MenuItemModel> _menuItems;

        private readonly ISettingsService _settingsService;
        private readonly IMenuService _menuService;
        private readonly INavigationService _navigationService;
        public ICommand SignOutCommand => new Command(async () => await SignOutAsync());


        public MenuViewModel(ISettingsService settingsService, IMenuService menuService, INavigationService navigationService)
        {
            _settingsService = settingsService;
            _menuService = menuService;
            _navigationService = navigationService;
        }

        public ObservableCollection<MenuItemModel> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        public ICommand ItemSelectedCommand => new Command<MenuItemModel>(SelectMenuItem);

        public override async Task InitializeAsync(object navigationData)
        {
            MenuItems = await _menuService.GetListAsync();
        }

        private async void SelectMenuItem(MenuItemModel item)
        {
            if (item.IsEnabled)
            {
                if (item.ViewModelType != null)
                {
                    await _navigationService.NavigateToAsync(item.ViewModelType, null);
                }
                else
                {
                    await SignOutAsync();
                }
            }
        }

        private async Task SignOutAsync()
        {
            IsBusy = true;

            await Task.Delay(500);

            _settingsService.AuthAccessToken = string.Empty;

            await _navigationService.NavigateToAsync<LoginViewModel>();

            IsBusy = false;
        }
    }
}
