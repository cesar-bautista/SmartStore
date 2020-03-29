﻿using System.Collections.ObjectModel;
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
    public sealed class CustomerViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<CustomerItemModel> _customer;
        private string _filter;
        private readonly INavigationService _navigationService;
        private readonly ICustomerService _customerService;
        #endregion

        #region Properties
        public ObservableCollection<CustomerItemModel> Customers
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

        public ICommand OnSelected => new Command<CategoryItemModel>(OnSelectedAction);
        public ICommand OnSearch => new Command(async () => { await OnSearchAction(); });
        #endregion

        #region Constructors
        public CustomerViewModel(INavigationService navigationService, ICustomerService customerService)
        {
            _navigationService = navigationService;
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
        private async void OnSelectedAction(object obj)
        {
            if (obj is CategoryItemModel item)
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
