using System.Collections.ObjectModel;
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
    public sealed class CategoryViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<CategoryItemModel> _categories;
        private string _filter;
        private readonly ICategoryService _categoryService;
        #endregion

        #region Properties
        public ObservableCollection<CategoryItemModel> Categories
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

        public ICommand OnSelected => new Command<CategoryItemModel>(OnSelectedAction);
        public ICommand OnSearch => new Command(async () => { await OnSearchAction(); });
        #endregion

        #region Constructors
        public CategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
        private async void OnSelectedAction(object obj)
        {
            if (obj is CategoryItemModel item)
            {

            }
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
