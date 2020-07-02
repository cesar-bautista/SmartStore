using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Models;

namespace SmartStore.App.Services.Business
{
    public class CategoryService : ICategoryService
    {
        public async Task<IEnumerable<CategoryModel>> GetListAsync()
        {
            await Task.Delay(1000);

            var list = new List<CategoryModel>();
            for (var i = 1; i < 11; i++)
            {
                list.Add(new CategoryModel
                {
                    Id = i,
                    Name = $"Category {i}",
                    Description = $"Description {i}"
                });
            }

            return list;
        }
    }
}
