﻿using System.Collections.ObjectModel;
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
    public sealed class SuppliersViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<SupplierModel> _suppliers;
        private string _filter;
        private readonly ISupplierService _supplierService;
        #endregion

        #region Properties
        public ObservableCollection<SupplierModel> Suppliers
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

        public ICommand OnSearch { get; }
        public ICommand OnSelected { get; }
        public ICommand OnAdd { get; }
        #endregion

        #region Constructors
        public SuppliersViewModel(ISupplierService supplierService)
        {
            _supplierService = supplierService;
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnSelected = new Command<SupplierModel>(async item => await OnSelectedAction(item));
            OnAdd = new Command(async () => await OnAddAction());
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
        private async Task OnSelectedAction(SupplierModel item)
        {
            IsBusy = true;
            item.IsReadOnly = true;
            await NavigationService.NavigateToAsync<SupplierViewModel>(item);
            IsBusy = false;
        }

        private async Task OnAddAction()
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<SupplierViewModel>();
            IsBusy = false;
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
