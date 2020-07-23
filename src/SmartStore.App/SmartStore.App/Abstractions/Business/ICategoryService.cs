using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Models;

namespace SmartStore.App.Abstractions.Business
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetListAsync();
        Task<CategoryModel> SaveAsync(CategoryModel model);
        Task<bool> DeleteAsync(CategoryModel model);
    }
}