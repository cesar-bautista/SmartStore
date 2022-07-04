using AutoMapper;
using SmartStore.App.Models;
using SmartStore.App.Services.Data.Entities;

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
            CreateMap<SaleEntity, OrderModel>()
                .ForMember(dest => dest.OrderNumber, opts => opts.MapFrom(src => src.SaleNumber))
                .ForMember(dest => dest.OrderDate, opts => opts.MapFrom(src => src.SaleDate))
                .ForMember(dest => dest.OrderDetails, opts => opts.MapFrom(src => src.SaleDetails));
            CreateMap<SaleDetailEntity, OrderDetailModel>();
        }
    }
}