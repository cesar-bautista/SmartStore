using System.Threading.Tasks;
using Acr.UserDialogs;
using SmartStore.App.Abstractions.Ui;

namespace SmartStore.App.Services.Ui
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title = "SmartStore", string buttonLabel = "Accept")
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public IProgressDialog ShowLoadingAsync(string message)
        {
            return UserDialogs.Instance.Loading(message, null, null, true, MaskType.Black);
        }
    }
}
