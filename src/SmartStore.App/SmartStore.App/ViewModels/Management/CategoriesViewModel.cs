using System.Collections.ObjectModel;
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
    public sealed class CategoriesViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<CategoryModel> _categories;
        private string _filter;
        private readonly ICategoryService _categoryService;
        #endregion

        #region Properties
        public ObservableCollection<CategoryModel> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
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
        public CategoriesViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnSelected = new Command<CategoryModel>(async item => await OnSelectedAction(item));
            OnAdd = new Command(async () => await OnAddAction());
            OnDelete = new Command<CategoryModel>(async item => await OnDeleteAction(item));
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _categoryService.GetListAsync();
            Categories = list.ToObservableCollection();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSelectedAction(CategoryModel item)
        {
            IsBusy = true;
            item.IsReadOnly = true;
            await NavigationService.NavigateToAsync<CategoryViewModel>(item);
            IsBusy = false;
        }

        private async Task OnAddAction()
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<CategoryViewModel>();
            IsBusy = false;
        }

        private async Task OnDeleteAction(CategoryModel item)
        {
            IsBusy = true;
            await _categoryService.DeleteAsync(item);
            await DialogService.ShowAlertAsync("Deleted...");
            IsBusy = false;
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            Categories = (await _categoryService.GetListAsync(Filter)).ToObservableCollection();
            IsBusy = false;
        } 
        #endregion
    }
}
