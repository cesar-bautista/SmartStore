using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Models;

namespace SmartStore.App.Abstractions.Business
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetListAsync(string filter = null);
        Task<IEnumerable<ProductModel>> GetFavoritesAsync();
        Task<ProductModel> SaveAsync(ProductModel model);
        Task<bool> DeleteAsync(ProductModel model);
    }
}