﻿using System;
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

            var images = new []
            {
                "https://cdn.pixabay.com/photo/2019/06/12/07/12/popcorn-4268489_960_720.jpg",
                "https://cdn.pixabay.com/photo/2014/09/12/18/20/coca-cola-443123_960_720.png",
                "https://cdn.pixabay.com/photo/2020/05/10/05/14/pepsi-5152332_960_720.jpg"
            };

            var list = new List<ProductItemModel>();
            var random = new Random();
            for (var i = 1; i < 11; i++)
            {
                list.Add(new ProductItemModel
                {
                    Id = i,
                    ImageUrl = images[random.Next(0, images.Length)],
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