using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;

namespace SmartStore.App.Services
{
    public class ProductService : IProductService
    {
        public async Task<ObservableCollection<ProductItemModel>> GetListAsync()
        {
            await Task.Delay(1000);

            return new ObservableCollection<ProductItemModel>
            {
                new ProductItemModel
                {
                    Id = 1,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1104/4168/products/Allbirds_W_Wool_Runner_Kotare_GREY_ANGLE_0f3bfe63-ac7d-4106-9acf-d26f8414ac53_600x600.png",
                    IsLike = true,
                    Price = 21.00,
                    Title = "Title"
                },
                new ProductItemModel
                {
                    Id = 2,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1104/4168/products/Allbirds_W_Wool_Runner_Kotare_GREY_ANGLE_0f3bfe63-ac7d-4106-9acf-d26f8414ac53_600x600.png",
                    IsLike = true,
                    Price = 88.12,
                    Title = "Title"
                },
                new ProductItemModel
                {
                    Id = 3,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1104/4168/products/Allbirds_W_Wool_Runner_Kotare_GREY_ANGLE_0f3bfe63-ac7d-4106-9acf-d26f8414ac53_600x600.png",
                    IsLike = true,
                    Price = 65.21,
                    Title = "Title"
                },
                new ProductItemModel
                {
                    Id = 4,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1104/4168/products/Allbirds_W_Wool_Runner_Kotare_GREY_ANGLE_0f3bfe63-ac7d-4106-9acf-d26f8414ac53_600x600.png",
                    IsLike = true,
                    Price = 39.95,
                    Title = "Title"
                },
                new ProductItemModel
                {
                    Id = 5,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1104/4168/products/Allbirds_W_Wool_Runner_Kotare_GREY_ANGLE_0f3bfe63-ac7d-4106-9acf-d26f8414ac53_600x600.png",
                    IsLike = true,
                    Price = 958.99,
                    Title = "Title"
                },
                new ProductItemModel
                {
                    Id = 6,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1104/4168/products/Allbirds_W_Wool_Runner_Kotare_GREY_ANGLE_0f3bfe63-ac7d-4106-9acf-d26f8414ac53_600x600.png",
                    IsLike = true,
                    Price = 64.85,
                    Title = "Title"
                },
                new ProductItemModel
                {
                    Id = 7,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1104/4168/products/Allbirds_W_Wool_Runner_Kotare_GREY_ANGLE_0f3bfe63-ac7d-4106-9acf-d26f8414ac53_600x600.png",
                    IsLike = true,
                    Price = 2050.55,
                    Title = "Title"
                },
                new ProductItemModel
                {
                    Id = 8,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/1104/4168/products/Allbirds_W_Wool_Runner_Kotare_GREY_ANGLE_0f3bfe63-ac7d-4106-9acf-d26f8414ac53_600x600.png",
                    IsLike = true,
                    Price = 120500.00,
                    Title = "Title"
                }
            };
        }
    }
}