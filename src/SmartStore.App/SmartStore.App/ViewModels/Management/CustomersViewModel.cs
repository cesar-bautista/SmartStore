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

        public ICommand OnSelected => new Command<CategoryModel>(OnSelectedAction);
        public ICommand OnSearch => new Command(async () => { await OnSearchAction(); });
        #endregion

        #region Constructors
        public CustomersViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
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
        private void OnSelectedAction(object obj)
        {
            if (obj is CategoryModel item)
            {

            }
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
