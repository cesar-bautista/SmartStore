using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions;
using SmartStore.App.Extensions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Management
{
    public sealed class SupplierViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<SupplierItemModel> _suppliers;
        private string _filter;
        private readonly INavigationService _navigationService;
        private readonly ISupplierService _supplierService;
        #endregion

        #region Properties
        public ObservableCollection<SupplierItemModel> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
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

        public ICommand OnSelected => new Command<SupplierItemModel>(OnSelectedAction);
        public ICommand OnSearch => new Command(async () => { await OnSearchAction(); });
        #endregion

        #region Constructors
        public SupplierViewModel(INavigationService navigationService, ISupplierService supplierService)
        {
            _navigationService = navigationService;
            _supplierService = supplierService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _supplierService.GetListAsync();
            Suppliers = list.ToObservableCollection();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async void OnSelectedAction(object obj)
        {
            if (obj is SupplierItemModel item)
            {

            }
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            var list = await _supplierService.GetListAsync();
            if (string.IsNullOrEmpty(this.Filter))
            {
                Suppliers = list.ToObservableCollection();
            }
            else
            {
                var products = list.Where(p =>
                        p.Name.ToLowerInvariant().Contains(Filter.ToLowerInvariant()));
                Suppliers = products.ToObservableCollection();
            }
            IsBusy = false;
        }
        #endregion
    }
}
