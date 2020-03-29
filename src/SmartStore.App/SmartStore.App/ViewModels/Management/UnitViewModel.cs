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
    public sealed class UnitViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<UnitItemModel> _units;
        private string _filter;
        private readonly INavigationService _navigationService;
        private readonly IUnitService _unitService;
        #endregion

        #region Properties
        public ObservableCollection<UnitItemModel> Units
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

        public ICommand OnSelected => new Command<UnitItemModel>(OnSelectedAction);
        public ICommand OnSearch => new Command(async () => { await OnSearchAction(); });
        #endregion

        #region Constructors
        public UnitViewModel(INavigationService navigationService, IUnitService unitService)
        {
            _navigationService = navigationService;
            _unitService = unitService;
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
        private async void OnSelectedAction(object obj)
        {
            if (obj is UnitItemModel item)
            {

            }
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
