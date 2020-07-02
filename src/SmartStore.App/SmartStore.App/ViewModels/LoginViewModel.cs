using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SmartStore.App.Abstractions;
using SmartStore.App.Abstractions.Core;
using SmartStore.App.ViewModels.Base;

namespace SmartStore.App.ViewModels
{
    public sealed class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private readonly ISettingsService _settingsService;
        private string _email;
        private string _password;
        #endregion

        #region Properties
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
                ((Command)OnLoginCommand).ChangeCanExecute();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ((Command)OnLoginCommand).ChangeCanExecute();
            }
        }
        
        public ICommand OnLoginCommand { get; }
        public ICommand OnForgotPasswordCommand { get; }
        public ICommand OnNewAccountCommand { get; }
        #endregion

        #region Constructors
        public LoginViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            OnLoginCommand = new Command(async () => await OnLoginAction(), CanLoginAction);
            OnForgotPasswordCommand = new Command(async () => await OnForgotPasswordAction());
            OnNewAccountCommand = new Command(async () => await OnNewAccountAction());
        }
        #endregion

        #region Actions

        private bool CanLoginAction()
        {
            return !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsBusy;
        }

        private async Task OnLoginAction()
        {
            IsBusy = true;

            //Simulate an API call to show busy / progress indicator
            await Task.Delay(1000).ContinueWith((t) => IsBusy = false);

            _settingsService.AuthAccessToken = "TOKEN";
            await NavigationService.NavigateToAsync<MainViewModel>();
        }

        private async Task OnForgotPasswordAction()
        {
            await DialogService.ShowAlertAsync("Forgot password action!!!");
        }

        private async Task OnNewAccountAction()
        {
            await DialogService.ShowAlertAsync("New account action!!!");
        }

        #endregion
    }
}