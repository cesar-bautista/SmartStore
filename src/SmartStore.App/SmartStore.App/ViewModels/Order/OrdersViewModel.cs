using SmartStore.App.Abstractions.Business;
using SmartStore.App.Extensions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using SmartStore.App.ViewModels.Terminal;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Order
{
    public sealed class OrdersViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<OrderModel> _orders;
        private string _filter;
        private readonly IOrderService _orderService;
        #endregion

        #region Properties
        public ObservableCollection<OrderModel> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
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
        public ICommand OnDelete { get; }
        #endregion

        #region Constructors
        public OrdersViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnSelected = new Command<OrderModel>(async item => await OnSelectedAction(item));
            OnDelete = new Command<OrderModel>(async item => await OnDeleteAction(item));
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _orderService.GetListAsync();
            Orders = list.ToObservableCollection();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSelectedAction(OrderModel item)
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<TerminalViewModel>(item.Id);
            IsBusy = false;
        }

        private async Task OnDeleteAction(OrderModel item)
        {
            IsBusy = true;
            await _orderService.DeleteAsync(item);
            await DialogService.ShowAlertAsync("Deleted...");
            IsBusy = false;
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            Orders = (await _orderService.GetListAsync(Filter)).ToObservableCollection();
            IsBusy = false;
        }
        #endregion
    }
}
