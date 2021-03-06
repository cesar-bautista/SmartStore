﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Extensions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Terminal
{
    public sealed class CheckoutViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<CheckoutModel> _shoppingCart;
        private ObservableCollection<CustomerModel> _customer;
        private readonly ICustomerService _customerService;
        private double _total;
        private double _paidAmount;
        private double _dueAmount;
        #endregion

        #region Properties
        public ObservableCollection<CheckoutModel> ShoppingCart
        {
            get => _shoppingCart;
            set
            {
                _shoppingCart = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CustomerModel> Customers
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        public double Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged();
            }
        }

        public double PaidAmount
        {
            get => _paidAmount;
            set
            {
                _paidAmount = value;
                OnPropertyChanged();
                DueAmount = value - Total;
                ((Command)OnPay).ChangeCanExecute();
            }
        }

        public double DueAmount
        {
            get => _dueAmount;
            set
            {
                _dueAmount = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnCancel { get; }
        public ICommand OnSave { get; }
        public ICommand OnPay { get; }
        #endregion

        #region Constructors
        public CheckoutViewModel(ICustomerService customerService)
        {
            _customerService = customerService;

            OnCancel = new Command(async () => { await OnCancelAction(); });
            OnSave = new Command(async () => { await OnSaveAction(); });
            OnPay = new Command(async () => { await OnPayAction(); }/*, CanPayAction*/);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is ObservableCollection<CheckoutModel> items)
            {
                IsBusy = true;

                ShoppingCart = items;
                PaidAmount = Total = items.Sum(s => s.Total);

                var list = await _customerService.GetListAsync();
                Customers = list.ToObservableCollection();

                IsBusy = false;
            }
        }
        #endregion

        #region Actions
        private async Task OnCancelAction()
        {
            await NavigationService.NavigateBackAsync();
        }

        private async Task OnSaveAction()
        {
            IsBusy = true;
            await DialogService.ShowAlertAsync("Saved");
            await NavigationService.NavigateToAsync<MainViewModel>(ShoppingCart);
            IsBusy = false;
        }

        private async Task OnPayAction()
        {
            IsBusy = true;
            await DialogService.ShowAlertAsync("Pay");
            await NavigationService.NavigateToAsync<MainViewModel>(ShoppingCart);
            IsBusy = false;
        }

        private bool CanPayAction()
        {
            var amount = Regex.Replace(PaidAmount.ToString(CultureInfo.InvariantCulture), @"\D", "");
            double.TryParse(amount, out var valueLong);
            return valueLong >= Total &&
                   !IsBusy;
        }
        #endregion
    }
}
