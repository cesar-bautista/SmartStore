using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Extensions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Management
{
    public sealed class ManagementsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<MenuModel> _managements;
        private readonly IMenuService _menuService;
        #endregion

        #region Properties
        public ObservableCollection<MenuModel> Managements
        {
            get => _managements;
            set
            {
                _managements = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnSelected => new Command<MenuModel>(OnSelectedAction);
        #endregion

        #region Constructors
        public ManagementsViewModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _menuService.GetManagementListAsync();
            Managements = list.ToObservableCollection();

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
