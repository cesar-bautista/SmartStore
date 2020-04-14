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

namespace SmartStore.App.ViewModels.Terminal
{
    public sealed class TerminalViewModel : BaseViewModel
    {
        #region Attributes
        private readonly IProductService _productService;
        private ObservableCollection<ProductItemModel> _products;
        private ObservableCollection<ProductItemModel> _shoppingCart;
        private string _filter;
        private string _commandText;
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

        public ObservableCollection<ProductItemModel> ShoppingCart
        {
            get => _shoppingCart;
            set
            {
                _shoppingCart = value;
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

        public ICommand OnSelected { get; }

        public ICommand OnSearch { get; }
        
        public ICommand OnDiscard { get; }

        public ICommand OnCheckout { get; }

        public string CheckoutText
        {
            get => _commandText;
            set
            {
                _commandText = $"{ShoppingCart.Count} Items = ${ShoppingCart.Sum(s => s.Price):0.##}";
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public TerminalViewModel(IProductService productService)
        {
            _productService = productService;
            OnSelected = new Command<ProductItemModel>(OnSelectedAction);
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnDiscard = new Command(OnDiscardAction);
            OnCheckout = new Command(OnCheckoutAction);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _productService.GetListAsync();
            Products = list.ToObservableCollection();
            ShoppingCart = new ObservableCollection<ProductItemModel>();
            CheckoutText = string.Empty;

            IsBusy = false;
        }
        #endregion

        #region Actions
        private void OnSelectedAction(object obj)
        {
            if (obj is ProductItemModel item)
            {
                ShoppingCart.Add(item);
                CheckoutText = string.Empty;
            }
        }

        private void OnDiscardAction()
        {
            ShoppingCart.Clear();
            CheckoutText = string.Empty;
        }

        private void OnCheckoutAction()
        {
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
