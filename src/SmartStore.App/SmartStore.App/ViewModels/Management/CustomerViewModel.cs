using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Management
{
    public class CustomerViewModel : BaseViewModel
    {
        #region Attributes
        private CustomerModel _customer;
        private readonly ICustomerService _customerService;
        #endregion

        #region Properties
        public CustomerModel Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }
        public ICommand OnSave { get; }
        public ICommand OnCancel { get; }
        #endregion

        #region Constructors
        public CustomerViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            OnSave = new Command(async () => { await OnSaveAction(); });
            OnCancel = new Command(async () => { await OnCancelAction(); });
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            if (navigationData is CustomerModel item)
                Customer = item;
            else
                Customer = new CustomerModel();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSaveAction()
        {
            IsBusy = true;
            await _customerService.SaveAsync(Customer);
            await DialogService.ShowAlertAsync("Saved...");
            await NavigationService.NavigateBackAsync(true);
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
