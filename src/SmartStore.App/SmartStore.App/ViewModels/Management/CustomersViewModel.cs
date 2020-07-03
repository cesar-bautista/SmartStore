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
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnSearch { get; }
        public ICommand OnSelected { get; }
        public ICommand OnAdd { get; }
        #endregion

        #region Constructors
        public CustomersViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnSelected = new Command<CustomerModel>(async item => await OnSelectedAction(item));
            OnAdd = new Command(async () => await OnAddAction());
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
            await NavigationService.NavigateToAsync<CustomerViewModel>(item);
            IsBusy = false;
        }

        private async Task OnAddAction()
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<CustomerViewModel>();
            IsBusy = false;
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            var list = await _customerService.GetListAsync();
            if (string.IsNullOrEmpty(this.Filter))
            {
                Customers = list.ToObservableCollection();
            }
            else
            {
                var products = list.Where(p =>
                        p.Name.ToLowerInvariant().Contains(Filter.ToLowerInvariant()));
                Customers = products.ToObservableCollection();
            }
            IsBusy = false;
        }
        #endregion
    }
}
