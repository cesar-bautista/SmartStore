using System.Collections.ObjectModel;
using System.Globalization;
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
    public sealed class ProductsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<ProductModel> _products;
        private string _filter;
        private readonly IProductService _productService;
        #endregion

        #region Properties
        public ObservableCollection<ProductModel> Products
        {
            get => _products;
            set
            {
                _products = value;
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
                //SearchAsync().GetAwaiter().GetResult();
            }
        }

        public ICommand OnSearch { get; }
        public ICommand OnSelected { get; }
        public ICommand OnAdd { get; }
        #endregion

        #region Constructors
        public ProductsViewModel(IProductService productService)
        {
            _productService = productService;
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnSelected = new Command<ProductModel>(async item => await OnSelectedAction(item));
            OnAdd = new Command(async () => await OnAddAction());
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _productService.GetListAsync();
            Products = list.ToObservableCollection();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSelectedAction(ProductModel item)
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<ProductViewModel>(item);
            IsBusy = false;
        }

        private async Task OnAddAction()
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<ProductViewModel>();
            IsBusy = false;
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            var list = await _productService.GetListAsync();
            if (string.IsNullOrEmpty(this.Filter))
            {
                Products = list.ToObservableCollection();
            }
            else
            {
                var products = list.Where(p =>
                        p.Name.ToLowerInvariant().Contains(Filter.ToLowerInvariant()) ||
                        p.Price.ToString(CultureInfo.InvariantCulture).ToLowerInvariant().Contains(Filter.ToLowerInvariant()));
                Products = products.ToObservableCollection();
            }
            IsBusy = false;
        } 
        #endregion
    }
}
