using System.Threading.Tasks;
using Acr.UserDialogs;
using SmartStore.App.Abstractions;

namespace SmartStore.App.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public IProgressDialog ShowLoadingAsync(string message)
        {
            return UserDialogs.Instance.Loading(message, null, null, true, MaskType.Black);
        }
    }
}
