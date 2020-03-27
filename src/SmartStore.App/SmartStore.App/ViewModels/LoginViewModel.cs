using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SmartStore.App.Abstractions;
using SmartStore.App.ViewModels.Base;

namespace SmartStore.App.ViewModels
{
    public sealed class LoginViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        #region Properties

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private bool _isShowCancel;
        public bool IsShowCancel
        {
            get => _isShowCancel;
            set
            {
                _isShowCancel = value;
                Task.Run(() => OnPropertyChanged());
                
            }
        }

        #endregion

        #region Commands

        public ICommand OnLoginCommand => new Command(async () => await OnLoginAction());
        public ICommand OnCancelLoginCommand => new Command(async () => await OnCancelLoginAction());
        public ICommand OnForgotPasswordCommand => new Command(async () => await OnForgotPasswordAction());
        public ICommand OnNewAccountCommand => new Command(async () => await OnNewAccountAction());

        #endregion

        public LoginViewModel(ISettingsService settingsService, INavigationService navigationService, IDialogService dialogService)
        {
            _settingsService = settingsService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }
        #region Methods

        private bool CanLoginAction()
        {
            return !string.IsNullOrWhiteSpace(this.Email) && !string.IsNullOrWhiteSpace(this.Password);
        }

        private async Task OnLoginAction()
        {
            IsBusy = true;

            //Show the Cancel button after X seconds
            await Task.Delay(3000).ContinueWith((t) => IsShowCancel = true);

            //Simulate an API call to show busy / progress indicator
            await Task.Delay(3000).ContinueWith((t) => IsBusy = false);

            _settingsService.AuthAccessToken = "TOKEN";
            await _navigationService.NavigateToAsync<MainViewModel>();
        }

        private async Task OnCancelLoginAction()
        {
            //TODO - perform cancellation logic
            IsBusy = false;
            IsShowCancel = false;
        }

        private async Task OnForgotPasswordAction()
        {
            await _dialogService.ShowAlertAsync("Forgot password action!!!", "Hi", "Accept");
        }

        private async Task OnNewAccountAction()
        {
            await _dialogService.ShowAlertAsync("New account action!!!", "Hi", "Accept");
        }

        #endregion
    }
}
