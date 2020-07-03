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
            set
            {
                _categories = value;
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
        public CategoriesViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnSelected = new Command<CategoryModel>(async item => await OnSelectedAction(item));
            OnAdd = new Command(async () => await OnAddAction());
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
            await NavigationService.NavigateToAsync<CategoryViewModel>(item);
            IsBusy = false;
        }

        private async Task OnAddAction()
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<CategoryViewModel>();
            IsBusy = false;
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            var list = await _categoryService.GetListAsync();
            if (string.IsNullOrEmpty(this.Filter))
            {
                Categories = list.ToObservableCollection();
            }
            else
            {
                var products = list.Where(p =>
                        p.Name.ToLowerInvariant().Contains(Filter.ToLowerInvariant()));
                Categories = products.ToObservableCollection();
            }
            IsBusy = false;
        } 
        #endregion
    }
}
