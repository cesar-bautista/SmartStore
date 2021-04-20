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
    public sealed class UnitsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<UnitModel> _units;
        private string _filter;
        private readonly IUnitService _unitService;
        #endregion

        #region Properties
        public ObservableCollection<UnitModel> Units
        {
            get => _units;
            set
            {
                _units = value;
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
        public ICommand OnDelete { get; }
        #endregion

        #region Constructors
        public UnitsViewModel(IUnitService unitService)
        {
            _unitService = unitService;
            OnSearch = new Command(async () => { await OnSearchAction(); });
            OnSelected = new Command<UnitModel>(async item => await OnSelectedAction(item));
            OnAdd = new Command(async () => await OnAddAction());
            OnDelete = new Command<UnitModel>(async item => await OnDeleteAction(item));
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            var list = await _unitService.GetListAsync();
            Units = list.ToObservableCollection();

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSelectedAction(UnitModel item)
        {
            IsBusy = true;
            item.IsReadOnly = true;
            await NavigationService.NavigateToAsync<UnitViewModel>(item);
            IsBusy = false;
        }

        private async Task OnAddAction()
        {
            IsBusy = true;
            await NavigationService.NavigateToAsync<UnitViewModel>();
            IsBusy = false;
        }

        private async Task OnDeleteAction(UnitModel item)
        {
            IsBusy = true;
            await _unitService.DeleteAsync(item);
            await DialogService.ShowAlertAsync("Deleted...");
            IsBusy = false;
        }

        private async Task OnSearchAction()
        {
            IsBusy = true;
            var list = await _unitService.GetListAsync();
            if (string.IsNullOrEmpty(this.Filter))
            {
                Units = list.ToObservableCollection();
            }
            else
            {
                var products = list.Where(p =>
                        p.Name.ToLowerInvariant().Contains(Filter.ToLowerInvariant()));
                Units = products.ToObservableCollection();
            }
            IsBusy = false;
        }
        #endregion
    }
}
