using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartStore.App.ViewModels.Base
{
    public class BaseViewModel : BindableObject
    {
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

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
