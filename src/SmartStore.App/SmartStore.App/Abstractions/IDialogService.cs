using System.Threading.Tasks;
using Acr.UserDialogs;

namespace SmartStore.App.Abstractions
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);

        IProgressDialog ShowLoadingAsync(string message);
    }
}
