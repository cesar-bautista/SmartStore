using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Models;

namespace SmartStore.App.Abstractions.Business
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitModel>> GetListAsync();
        Task<UnitModel> SaveAsync(UnitModel model);
        Task<bool> DeleteAsync(UnitModel model);
    }
}
