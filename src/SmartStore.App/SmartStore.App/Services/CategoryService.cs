using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;

namespace SmartStore.App.Services
{
    public class CategoryService : ICategoryService
    {
        public async Task<IEnumerable<CategoryItemModel>> GetListAsync()
        {
            await Task.Delay(1000);

            var list = new List<CategoryItemModel>();
            for (var i = 1; i < 11; i++)
            {
                list.Add(new CategoryItemModel
                {
                    Id = i,
                    Name = $"Category {i}",
                    Description = $"Category {i}"
                });
            }

            return list;
        }
    }
}
