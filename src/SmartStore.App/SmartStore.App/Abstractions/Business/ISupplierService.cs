using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Models;

namespace SmartStore.App.Abstractions.Business
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierModel>> GetListAsync();
        Task<SupplierModel> SaveAsync(SupplierModel model);
        Task<bool> DeleteAsync(SupplierModel model);
    }
}