using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions;
using SmartStore.App.Extensions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Management
{
    public sealed class ManagementViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<ManagementModel> _managements;
        private readonly IManagementService _managementService;
        #endregion

        #region Properties
        public ObservableCollection<ManagementModel> Managements
        {
            get => _managements;
            set
            {
                _managements = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnSelected => new Command<ManagementModel>(OnSelectedAction);
        #endregion

        #region Constructors
        public ManagementViewModel(IManagementService managementService)
        {
            _managementService = managementService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _managementService.GetListAsync();
            Managements = list.ToObservableCollection();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async void OnSelectedAction(object obj)
        {
            if (!(obj is ManagementModel item)) return;
            switch (item.Id)
            {
                case "PROD":
                    await NavigationService.NavigateToAsync<ProductViewModel>();
                    break;
                case "CATE":
                    await NavigationService.NavigateToAsync<CategoryViewModel>();
                    break;
                case "UNIT":
                    await NavigationService.NavigateToAsync<UnitViewModel>();
                    break;
                case "CUST":
                    await NavigationService.NavigateToAsync<CustomerViewModel>();
                    break;
                case "SUPP":
                    await NavigationService.NavigateToAsync<SupplierViewModel>();
                    break;
            }
        } 
        #endregion
    }
}
