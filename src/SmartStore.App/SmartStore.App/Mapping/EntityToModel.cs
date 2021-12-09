using AutoMapper;
using SmartStore.App.Models;
using SmartStore.App.Services.Data.Entities;
using System.Linq;

namespace SmartStore.App.Mapping
{
    public class EntityToModel : Profile
    {
        public EntityToModel()
        {
            CreateMap<CategoryEntity, CategoryModel>();
            CreateMap<CustomerEntity, CustomerModel>();
            CreateMap<ProductEntity, ProductModel>();
            CreateMap<SupplierEntity, SupplierModel>();
            CreateMap<UnitEntity, UnitModel>();
            CreateMap<OrderEntity, OrderModel>();
            CreateMap<OrderDetailEntity, OrderDetailModel>();
        }
    }
}