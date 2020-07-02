using System.Threading.Tasks;
using Acr.UserDialogs;

namespace SmartStore.App.Abstractions.Ui
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title = "SmartStore", string buttonLabel = "Accept");

        IProgressDialog ShowLoadingAsync(string message);
    }
}
