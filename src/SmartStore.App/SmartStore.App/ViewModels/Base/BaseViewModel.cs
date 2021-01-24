using System.Runtime.CompilerServices;
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

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
