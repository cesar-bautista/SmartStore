using System.Collections.ObjectModel;
using System.Globalization;
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
    public sealed class ProductViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<ProductItemModel> _products;
        private string _filter;
        private readonly INavigationService _navigationService;
        private readonly IProductService _productService;
        #endregion

        #region Properties
        public ObservableCollection<ProductItemModel> Products
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

        public ICommand OnProductItemSelected => new Command<ProductItemModel>(ProductItemSelected);
        public ICommand OnSearchCommand => new Command(async () => { await SearchAsync(); });
        #endregion

        #region Constructors
        public ProductViewModel(INavigationService navigationService, IProductService productService)
        {
            _navigationService = navigationService;
            _productService = productService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            Products = await _productService.GetListAsync();

            IsBusy = false;
        }
        #endregion

        private async void ProductItemSelected(object obj)
        {
            if (obj is ProductItemModel product)
            {

            }
        }

        private async Task SearchAsync()
        {
            IsBusy = true;
            if (string.IsNullOrEmpty(this.Filter))
            {
                Products = await _productService.GetListAsync();
            }
            else
            {
                var allProducts = await _productService.GetListAsync();
                var products = allProducts.Where(p =>
                        p.Title.ToLowerInvariant().Contains(Filter.ToLowerInvariant()) ||
                        p.Price.ToString(CultureInfo.InvariantCulture).ToLowerInvariant().Contains(Filter.ToLowerInvariant()));
                Products = products.ToObservableCollection();
            }
            IsBusy = false;
        }
    }
}
