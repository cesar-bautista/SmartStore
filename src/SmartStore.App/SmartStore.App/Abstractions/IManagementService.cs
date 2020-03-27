using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SmartStore.App.Models;

namespace SmartStore.App.Abstractions
{
    public interface IManagementService
    {
        Task<ObservableCollection<ManagementItemModel>> GetListAsync();
    }
}
