using System.Threading.Tasks;

namespace SmartStore.App.Abstractions.Business
{
    public interface ISyncService
    {
        Task Initialize();

        Task<bool> Sync();
    }
}