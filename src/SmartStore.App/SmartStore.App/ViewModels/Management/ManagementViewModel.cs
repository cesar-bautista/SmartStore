using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Management
{
    public sealed class ManagementViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<ManagementItemModel> _managements;
        private readonly INavigationService _navigationService;
        private readonly IManagementService _managementService;
        #endregion

        #region Properties
        public ObservableCollection<ManagementItemModel> Managements
        {
            get => _managements;
            set
            {
                _managements = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnManagementItemSelected => new Command<ManagementItemModel>(ManagementItemSelected);
        #endregion

        #region Constructors
        public ManagementViewModel(INavigationService navigationService, IManagementService managementService)
        {
            _navigationService = navigationService;
            _managementService = managementService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            Managements = await _managementService.GetListAsync();

            IsBusy = false;
        }
        #endregion

        private async void ManagementItemSelected(object obj)
        {
            if (obj is ManagementItemModel management)
            {
                switch (management.Id)
                {
                    case "PROD":
                        await _navigationService.NavigateToAsync<ProductViewModel>();
                        break;
                    case "CATE":
                        await _navigationService.NavigateToAsync<CategoryViewModel>();
                        break;
                    case "UNIT":
                        await _navigationService.NavigateToAsync<UnitViewModel>();
                        break;
                    case "CUST":
                        await _navigationService.NavigateToAsync<CustomerViewModel>();
                        break;
                    case "SUPP":
                        await _navigationService.NavigateToAsync<SupplierViewModel>();
                        break;
                }
            }
        }
    }
}
