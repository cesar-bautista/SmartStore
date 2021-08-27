using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Management
{
    public class CategoryViewModel : BaseViewModel
    {
        #region Attributes
        private CategoryModel _category;
        private readonly ICategoryService _categoryService;
        #endregion

        #region Properties
        public CategoryModel Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }
        public ICommand OnSave { get; }
        public ICommand OnCancel { get; }
        #endregion

        #region Constructors
        public CategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            OnSave = new Command(async () => { await OnSaveAction(); });
            OnCancel = new Command(async () => { await OnCancelAction(); });
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            if (navigationData is CategoryModel item)
                Category = item;
            else
                Category = new CategoryModel();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSaveAction()
        {
            IsBusy = true;
            await _categoryService.SaveAsync(Category);
            await DialogService.ShowAlertAsync("Saved...");
            await NavigationService.NavigateBackAsync(true);
            IsBusy = false;
        }

        private async Task OnCancelAction()
        {
            IsBusy = true;
            await NavigationService.NavigateBackAsync();
            IsBusy = false;
        }
        #endregion
    }
}
