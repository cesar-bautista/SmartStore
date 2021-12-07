using SmartStore.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartStore.App.Abstractions.Business
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetListAsync(string filter = null);
        Task<OrderModel> SaveAsync(IEnumerable<OrderModel> model);
        Task<bool> DeleteAsync(OrderModel model);
    }
}