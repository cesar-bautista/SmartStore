using System.Collections.Generic;
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
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace SmartStore.App.ViewModels.Terminal
{
    public sealed class TerminalViewModel : BaseViewModel
    {
        #region Attributes
        private readonly IProductService _productService;
        private ObservableCollection<ProductModel> _products;
        private ObservableCollection<CheckoutModel> _shoppingCart;
        private string _filter;
        private string _commandText;
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

        public ObservableCollection<CheckoutModel> ShoppingCart
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

        public ICommand OnBarcode { get; }

        public ICommand OnDiscard { get; }

        public ICommand OnCheckout { get; }

        public string CheckoutText
        {
            get => _commandText;
            set
            {
                _commandText = $"{ShoppingCart.Sum(s => s.Quantity)} Items = ${ShoppingCart.Sum(s => s.Price):0.##}";
                OnPropertyChanged();
                ((Command)OnDiscard).ChangeCanExecute();
                ((Command)OnCheckout).ChangeCanExecute();
            }
        }
        #endregion

        #region Constructors
        public TerminalViewModel(IProductService productService)
        {
            _productService = productService;
            OnSelected = new Command<ProductModel>(OnSelectedAction);
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnBarcode = new Command(OnBarcodeAction);
            OnDiscard = new Command(OnDiscardAction, CanExcecuteAction);
            OnCheckout = new Command(async () => { await OnCheckoutAction(); }, CanExcecuteAction);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _productService.GetListAsync();
            Products = list.ToObservableCollection();
            ShoppingCart = new ObservableCollection<CheckoutModel>();
            CheckoutText = string.Empty;

            IsBusy = false;
        }
        #endregion

        #region Actions
        private bool CanExcecuteAction()
        {
            return ShoppingCart != null &&
                   ShoppingCart.Any() &&
                   !IsBusy;
        }

        private void OnSelectedAction(object obj)
        {
            if (obj is ProductModel item)
            {
                var element = ShoppingCart.FirstOrDefault(e => e.Id == item.Id);
                if (element != null)
                {
                    element.Quantity++;
                    element.Price = item.Price * element.Quantity;
                }
                else
                {
                    element = ToModelMap(item);
                    ShoppingCart.Add(element);
                }

                CheckoutText = string.Empty;
            }
        }

        private void OnDiscardAction()
        {
            ShoppingCart.Clear();
            CheckoutText = string.Empty;
        }

        private async Task OnCheckoutAction()
        {
            await NavigationService.NavigateToAsync<CheckoutViewModel>(ShoppingCart);
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

        private void OnBarcodeAction()
        {
            var options = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<BarcodeFormat>
                {
                    BarcodeFormat.QR_CODE,
                    BarcodeFormat.CODE_128,
                    BarcodeFormat.EAN_13
                }
            };

            var page = new ZXingScannerPage(options) { Title = "Scanner" };
            var closeItem = new ToolbarItem { Text = "Close" };
            closeItem.Clicked += (sender, args) =>
            {
                page.IsScanning = false;
                Device.BeginInvokeOnMainThread(() => { Application.Current.MainPage.Navigation.PopModalAsync(); });
            };

            page.ToolbarItems.Add(closeItem);
            page.OnScanResult += (result) =>
            {
                page.IsScanning = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                    Filter = string.IsNullOrEmpty(result.Text) ? "No valid code has been scanned" : result.Text;
                });
            };
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(page) { BarTextColor = Color.White, BarBackgroundColor = Color.FromRgb(30, 38, 52) }, true);
        }
        #endregion

        #region Methods
        public static CheckoutModel ToModelMap(ProductModel item)
        {
            return new CheckoutModel
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Description = item.Description,
                Cost = item.Cost,
                Price = item.Price,
                Stock = item.Stock,
                Quantity = 1,
                ImageUrl = item.ImageUrl
            };
        }
        #endregion
    }
}
