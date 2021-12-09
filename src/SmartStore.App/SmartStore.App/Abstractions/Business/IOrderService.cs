using SmartStore.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartStore.App.Abstractions.Business
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetListAsync(string filter = null);
        Task<OrderModel> GetListWithChildrenAsync(Guid filter);
        Task SaveAsync(OrderModel model);
        Task<bool> DeleteAsync(OrderModel model);
    }
}