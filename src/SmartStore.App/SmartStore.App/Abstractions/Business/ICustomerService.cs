using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Models;

namespace SmartStore.App.Abstractions.Business
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerModel>> GetListAsync(string filter = null);
        Task<CustomerModel> SaveAsync(CustomerModel model);
        Task<bool> DeleteAsync(CustomerModel model);
    }
}