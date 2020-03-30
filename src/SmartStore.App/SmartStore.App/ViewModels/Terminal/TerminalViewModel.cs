﻿using System.Collections.ObjectModel;
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
        private ObservableCollection<ProductItemModel> _products;
        private string _filter;
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

        public ICommand OnSelected => new Command<ProductItemModel>(OnSelectedAction);
        public ICommand OnSearch => new Command(async () => { await OnSearchAction(); });
        #endregion

        #region Constructors
        public TerminalViewModel(IProductService productService)
        {
            _productService = productService;
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
        private async void OnSelectedAction(object obj)
        {
            if (obj is ProductItemModel item)
            {

            }
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