using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;

namespace SmartStore.App.Services
{
    public class ProductService : IProductService
    {
        public async Task<IEnumerable<ProductItemModel>> GetListAsync()
        {
            await Task.Delay(1000);

            var list = new List<ProductItemModel>();
            var random = new Random();
            for (var i = 1; i < 11; i++)
            {
                list.Add(new ProductItemModel
                {
                    Id = i,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1104/4168/products/Allbirds_W_Wool_Runner_Kotare_GREY_ANGLE_0f3bfe63-ac7d-4106-9acf-d26f8414ac53_600x600.png",
                    Price = random.NextDouble() * (100 - i) + i,
                    Name = $"Product {i}",
                    Description = $"Product {i}",
                    Code = "00" + i,
                    Cost = random.NextDouble() * (100 - i) + i,
                    MinStock = 5,
                    Stock = random.Next() * (100 - i) + i,
                    SupplierId = 1,
                    CategoryId = 1,
                    UnitId = 1,
                    IsFavorite = random.Next(i, 100) < 10
                });
            }

            return list;
        }
    }
}