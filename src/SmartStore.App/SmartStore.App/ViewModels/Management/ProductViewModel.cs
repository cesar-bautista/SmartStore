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
    public class ProductViewModel : BaseViewModel
    {
        #region Attributes
        private ProductModel _product;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitService _unitService;
        private readonly ISupplierService _supplierService;
        private ObservableCollection<CategoryModel> _categories;
        private ObservableCollection<SupplierModel> _suppliers;
        private ObservableCollection<UnitModel> _units;
        private CategoryModel _selectedCategory;
        private UnitModel _selectedUnit;
        private SupplierModel _selectedSupplier;

        #endregion

        #region Properties
        public ProductModel Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CategoryModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }
        public CategoryModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UnitModel> Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged();
            }
        }
        public UnitModel SelectedUnit
        {
            get => _selectedUnit;
            set
            {
                _selectedUnit = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<SupplierModel> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                OnPropertyChanged();
            }
        }
        public SupplierModel SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged();
            }
        }
        public ICommand OnSave { get; }
        public ICommand OnCancel { get; }
        public ICommand OnFavorite { get; }
        #endregion

        #region Constructors
        public ProductViewModel(IProductService productService, ICategoryService categoryService, IUnitService unitService, ISupplierService supplierService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _unitService = unitService;
            _supplierService = supplierService;
            OnSave = new Command(async () => { await OnSaveAction(); });
            OnCancel = new Command(async () => { await OnCancelAction(); });
            OnFavorite = new Command(OnFavoriteAction);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var suppliers = await _supplierService.GetListAsync();
            Suppliers = suppliers.ToObservableCollection();

            var categories = await _categoryService.GetListAsync();
            Categories = categories.ToObservableCollection();

            var units = await _unitService.GetListAsync();
            Units = units.ToObservableCollection();

            if (navigationData is ProductModel item)
            {
                Product = item;

                SelectedCategory = Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                SelectedUnit = Units.FirstOrDefault(c => c.Id == item.UnitId);
                SelectedSupplier = Suppliers.FirstOrDefault(c => c.Id == item.SupplierId);
            }
            else
                Product = new ProductModel();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSaveAction()
        {
            IsBusy = true;
            Product.CategoryId = SelectedCategory != null ? SelectedCategory.Id : System.Guid.Empty;
            Product.UnitId = SelectedUnit != null ? SelectedUnit.Id : System.Guid.Empty;
            Product.SupplierId = SelectedSupplier != null ? SelectedSupplier.Id : System.Guid.Empty;
            await _productService.SaveAsync(Product);
            await DialogService.ShowAlertAsync("Saved...");
            await NavigationService.NavigateBackAsync();
            IsBusy = false;
        }

        private async Task OnCancelAction()
        {
            IsBusy = true;
            await NavigationService.NavigateBackAsync();
            IsBusy = false;
        }

        private void OnFavoriteAction()
        {
            Product.IsFavorite = !Product.IsFavorite;
            Product = Product;
        }
        #endregion
    }
}
