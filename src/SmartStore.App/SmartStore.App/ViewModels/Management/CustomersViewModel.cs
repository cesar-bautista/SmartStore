using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Extensions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Management
{
    public sealed class CustomersViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<CustomerModel> _customer;
        private string _filter;
        private readonly ICustomerService _customerService;
        #endregion

        #region Properties
        public ObservableCollection<CustomerModel> Customers
        {
            get => _customer;
            set => SetProperty(ref _customer, value);
        }

        public string Filter
        {
            get => _filter;
            set
            {
                SetProperty(ref _filter, value);
                Task.Run(() => OnSearchAction());
            }
        }

        public ICommand OnSearch { get; }
        public ICommand OnSelected { get; }
        public ICommand OnAdd { get; }
        public ICommand OnDelete { get; }
        #endregion

        #region Constructors
        public CustomersViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnSelected = new Command<CustomerModel>(async item => await OnSelectedAction(item));
            OnAdd = new Command(async () => await OnAddAction());
            OnDelete = new Command<CustomerModel>(async item => await OnDeleteAction(item));
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _customerService.GetListAsync();
            Customers = list.ToObservableCollection();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSelectedAction(CustomerModel item)
        {
            IsBusy = true;
            item.IsReadOnly = true;
            await NavigationService.NavigateToAsync<CustomerViewModel>(item);
            IsBusy = false;
        }

        private async Task OnAddAction()
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<CustomerViewModel>();
            IsBusy = false;
        }

        private async Task OnDeleteAction(CustomerModel item)
        {
            IsBusy = true;
            await _customerService.DeleteAsync(item);
            await DialogService.ShowAlertAsync("Deleted...");
            IsBusy = false;
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            Customers = (await _customerService.GetListAsync(Filter)).ToObservableCollection();
            IsBusy = false;
        }
        #endregion
    }
}
