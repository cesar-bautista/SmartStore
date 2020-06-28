using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;

namespace SmartStore.App.Services
{
    public class ManagementService : IManagementService
    {
        public async Task<IEnumerable<ManagementModel>> GetListAsync()
        {
            await Task.Delay(1000);

            return new List<ManagementModel>
            {
                new ManagementModel
                {
                    Id = "PROD",
                    ImageUrl = "https://img.icons8.com/color/48/000000/product.png",
                    Title = "Products",
                    Description = "View list, edit, create and delete"
                },
                new ManagementModel
                {
                    Id = "CATE",
                    ImageUrl = "https://img.icons8.com/color/48/000000/opened-folder.png",
                    Title = "Categories",
                    Description = "View list, edit, create and delete"
                }
                ,
                new ManagementModel
                {
                    Id = "UNIT",
                    ImageUrl = "https://img.icons8.com/color/48/000000/puzzle.png",
                    Title = "Units",
                    Description = "View list, edit, create and delete"
                },
                new ManagementModel
                {
                    Id = "CUST",
                    ImageUrl = "https://img.icons8.com/color/48/000000/group.png",
                    Title = "Customers",
                    Description = "View list, edit, create and delete"
                },
                new ManagementModel
                {
                    Id = "SUPP",
                    ImageUrl = "https://img.icons8.com/color/48/000000/supplier.png",
                    Title = "Suppliers",
                    Description = "View list, edit, create and delete"
                }
            };
        }
    }
}
