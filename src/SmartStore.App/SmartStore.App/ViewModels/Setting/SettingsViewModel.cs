using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Extensions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Setting
{
    public sealed class SettingsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<MenuModel> _settings;
        private readonly IMenuService _menuService;
        #endregion

        #region Properties
        public ObservableCollection<MenuModel> Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnSelected => new Command<MenuModel>(OnSelectedAction);
        #endregion

        #region Constructors
        public SettingsViewModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _menuService.GetSettingListAsync();
            Settings = list.ToObservableCollection();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async void OnSelectedAction(MenuModel item)
        {
            if (!item.IsEnabled) return;
            IsBusy = true;
            await NavigationService.NavigateToAsync(item.ViewModelType);
            IsBusy = false;
        }
        #endregion
    }
}