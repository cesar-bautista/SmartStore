using System.Collections.Generic;
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
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace SmartStore.App.ViewModels.Terminal
{
    public sealed class TerminalViewModel : BaseViewModel
    {
        #region Attributes
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private ObservableCollection<ProductModel> _products;
        private OrderModel _shoppingCart;
        private string _filter;
        private string _commandText;
        #endregion

        #region Properties
        public ObservableCollection<ProductModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public OrderModel ShoppingCart
        {
            get => _shoppingCart;
            set => SetProperty(ref _shoppingCart, value);
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
                _commandText = $"{ShoppingCart.OrderDetails.Sum(s => s.Quantity)} Items = ${ShoppingCart.OrderDetails.Sum(s => s.Total):0.##}";
                OnPropertyChanged();
                ((Command)OnDiscard).ChangeCanExecute();
                ((Command)OnCheckout).ChangeCanExecute();
            }
        }
        #endregion

        #region Constructors
        public TerminalViewModel(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
            OnSelected = new Command<ProductModel>(OnSelectedAction);
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnBarcode = new Command(OnBarcodeAction);
            OnDiscard = new Command(OnDiscardAction, CanExcecuteAction);
            OnCheckout = new Command(async () => { await OnCheckoutAction(); }, CanExcecuteAction);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            Products = (await _productService.GetFavoritesAsync()).ToObservableCollection();
            
            if (navigationData is System.Guid id)
                ShoppingCart = await _orderService.GetListWithChildrenAsync(id);
            else
                ShoppingCart = new OrderModel() { OrderDetails = new List<OrderDetailModel>() };

            CheckoutText = string.Empty;

            IsBusy = false;
        }
        #endregion

        #region Actions
        private bool CanExcecuteAction()
        {
            return ShoppingCart != null &&
                   ShoppingCart.OrderDetails.Any() &&
                   !IsBusy;
        }

        private void OnSelectedAction(ProductModel item)
        {
            var element = ShoppingCart.OrderDetails.FirstOrDefault(e => e.Id == item.Id);
            if (element != null)
            {
                element.Quantity++;
            }
            else
            {
                element = ToModelMap(item);
                ShoppingCart.OrderDetails.Add(element);
            }

            CheckoutText = string.Empty;
        }

        private void OnDiscardAction()
        {
            IsBusy = true;
            ShoppingCart.OrderDetails.Clear();
            CheckoutText = string.Empty;
            IsBusy = false;
        }

        private async Task OnCheckoutAction()
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<CheckoutViewModel>(ShoppingCart);
            IsBusy = false;
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            Products = string.IsNullOrWhiteSpace(Filter) ?
                (await _productService.GetFavoritesAsync()).ToObservableCollection() :
                (await _productService.GetListAsync(Filter)).ToObservableCollection();
            IsBusy = false;
        }

        private void OnBarcodeAction()
        {
            IsBusy = true;
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
            IsBusy = false;
        }
        #endregion

        #region Methods
        //TODO: Enviarlo a AutoMapper
        public static OrderDetailModel ToModelMap(ProductModel item)
        {
            return new OrderDetailModel
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
