using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Models;

namespace SmartStore.App.Abstractions.Business
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetListAsync();
    }
}
