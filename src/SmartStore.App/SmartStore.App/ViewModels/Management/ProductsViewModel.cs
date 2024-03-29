﻿using System.Collections.ObjectModel;
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
            set => SetProperty(ref _products, value);
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
        public ICommand OnAdd { get; }
        public ICommand OnDelete { get; }
        #endregion

        #region Constructors
        public ProductsViewModel(IProductService productService)
        {
            _productService = productService;
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnSelected = new Command<ProductModel>(async item => await OnSelectedAction(item));
            OnAdd = new Command(async () => await OnAddAction());
            OnDelete = new Command<ProductModel>(async item => await OnDeleteAction(item));
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            Products = (await _productService.GetListAsync()).ToObservableCollection();
            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSelectedAction(ProductModel item)
        {
            IsBusy = true;
            item.IsReadOnly = true;
            await NavigationService.NavigateToAsync<ProductViewModel>(item);
            IsBusy = false;
        }

        private async Task OnAddAction()
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<ProductViewModel>();
            IsBusy = false;
        }

        private async Task OnDeleteAction(ProductModel item)
        {
            IsBusy = true;
            await _productService.DeleteAsync(item);
            await DialogService.ShowAlertAsync("Deleted...");
            IsBusy = false;
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            Products = (await _productService.GetListAsync(Filter)).ToObservableCollection();
            IsBusy = false;
        } 
        #endregion
    }
}
