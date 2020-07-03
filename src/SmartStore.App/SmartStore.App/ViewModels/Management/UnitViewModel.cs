using System.Threading.Tasks;
using System.Windows.Input;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Management
{
    public class UnitViewModel : BaseViewModel
    {
        #region Attributes
        private UnitModel _unit;
        private readonly IUnitService _unitService;
        #endregion

        #region Properties
        public UnitModel Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                OnPropertyChanged();
            }
        }
        public ICommand OnSave { get; }
        public ICommand OnCancel { get; }
        #endregion

        #region Constructors
        public UnitViewModel(IUnitService unitService)
        {
            _unitService = unitService;
            OnSave = new Command(async () => { await OnSaveAction(); });
            OnCancel = new Command(async () => { await OnCancelAction(); });
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            if (navigationData is UnitModel item)
            {
                Unit = item;
            }

            IsBusy = false;
        }
        #endregion

        #region Actions
        private async Task OnSaveAction()
        {
            IsBusy = true;
            await DialogService.ShowAlertAsync("Saving...");
            await NavigationService.NavigateBackAsync();
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