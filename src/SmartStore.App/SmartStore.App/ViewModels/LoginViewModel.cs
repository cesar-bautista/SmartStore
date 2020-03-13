using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SmartStore.App.Abstractions;
using SmartStore.App.ViewModels.Base;

namespace SmartStore.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
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

        public ICommand LoginCommand => new Command(async () => await LoginAction());
        public ICommand CancelLoginCommand => new Command(async () => await CancelLoginAction());
        public ICommand ForgotPasswordCommand => new Command(async () => await ForgotPasswordAction());
        public ICommand NewAccountCommand => new Command(async () => await NewAccountAction());

        #endregion

        public LoginViewModel(ISettingsService settingsService, INavigationService navigationService, IDialogService dialogService)
        {
            _settingsService = settingsService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        //public override async Task InitializeAsync(object navigationData)
        //{
        //using (_dialogService.ShowLoadingAsync("Loading"))
        //{
        //    await Task.Delay(5000);
        //}

        //_settingsService.AuthAccessToken = "TOKEN";
        //await _navigationService.NavigateToAsync<MainViewModel>();
        //}

        #region Methods

        private bool CanLoginAction()
        {
            return !string.IsNullOrWhiteSpace(this.Email) && !string.IsNullOrWhiteSpace(this.Password);
        }

        private async Task LoginAction()
        {
            IsBusy = true;

            //await _dialogService.ShowAlertAsync("Login action!!!", "Hi", "Accept");

            //Show the Cancel button after X seconds
            await Task.Delay(3000).ContinueWith((t) => IsShowCancel = true);

            //Simulate an API call to show busy / progress indicator
            await Task.Delay(5000).ContinueWith((t) => IsBusy = false);

            _settingsService.AuthAccessToken = "TOKEN";
            await _navigationService.NavigateToAsync<MainViewModel>();
        }

        private async Task CancelLoginAction()
        {
            //TODO - perform cancellation logic
            IsBusy = false;
            IsShowCancel = false;
        }

        private async Task ForgotPasswordAction()
        {
            await _dialogService.ShowAlertAsync("Forgot password action!!!", "Hi", "Accept");
        }

        private async Task NewAccountAction()
        {
            await _dialogService.ShowAlertAsync("New account action!!!", "Hi", "Accept");
        }

        #endregion
    }
}
