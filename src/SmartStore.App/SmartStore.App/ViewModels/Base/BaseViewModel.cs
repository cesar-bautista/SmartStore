using System.Threading.Tasks;
using SmartStore.App.Abstractions.Ui;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Base
{
    public abstract class BaseViewModel : BindableObject
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        protected BaseViewModel()
        {
            DialogService = LocatorViewModel.Instance.Resolve<IDialogService>();
            NavigationService = LocatorViewModel.Instance.Resolve<INavigationService>();
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
