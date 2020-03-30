using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions;
using SmartStore.App.Extensions;
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

        public ICommand SignOutCommand => new Command(async () => await SignOutAsync());
        public ICommand ItemSelectedCommand => new Command<MenuItemModel>(SelectMenuItem);

        public MenuViewModel(ISettingsService settingsService, IMenuService menuService)
        {
            _settingsService = settingsService;
            _menuService = menuService;
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

        public override async Task InitializeAsync(object navigationData)
        {
            var list = await _menuService.GetListAsync();
            MenuItems = list.ToObservableCollection();
        }

        private async void SelectMenuItem(MenuItemModel item)
        {
            if (!item.IsEnabled) return;
            if (item.ViewModelType != null)
            {
                await NavigationService.NavigateToAsync(item.ViewModelType, null);
            }
            else
            {
                await SignOutAsync();
            }
        }

        private async Task SignOutAsync()
        {
            IsBusy = true;

            _settingsService.AuthAccessToken = string.Empty;

            await NavigationService.NavigateToAsync<LoginViewModel>();
            await NavigationService.RemoveLastFromBackStackAsync();

            IsBusy = false;
        }
    }
}
