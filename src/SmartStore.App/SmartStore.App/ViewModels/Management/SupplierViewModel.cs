using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Management
{
    public class SupplierViewModel : BaseViewModel
    {
        #region Attributes
        private SupplierModel _supplier;
        private readonly ISupplierService _supplierService;
        #endregion

        #region Properties
        public SupplierModel Supplier
        {
            get => _supplier;
            set
            {
                _supplier = value;
                OnPropertyChanged();
            }
        }
        public ICommand OnSave { get; }
        public ICommand OnCancel { get; }
        #endregion

        #region Constructors
        public SupplierViewModel(ISupplierService supplierService)
        {
            _supplierService = supplierService;
            OnSave = new Command(async () => { await OnSaveAction(); });
            OnCancel = new Command(async () => { await OnCancelAction(); });
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            if (navigationData is SupplierModel item)
            {
                Supplier = item;
            }

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSaveAction()
        {
            IsBusy = true;
            await DialogService.ShowAlertAsync("Saving...");
            await NavigationService.NavigateBackAsync();
            IsBusy = false;
        }

        private async Task OnCancelAction()
        {
            IsBusy = true;
            await NavigationService.NavigateBackAsync();
            IsBusy = false;
        }
        #endregion
    }
}