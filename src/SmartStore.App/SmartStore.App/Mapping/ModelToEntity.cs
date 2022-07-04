using AutoMapper;
using SmartStore.App.Models;
using SmartStore.App.Services.Data.Entities;

namespace SmartStore.App.Mapping
{
    public class ModelToEntity : Profile
    {
        public ModelToEntity()
        {
            CreateMap<CategoryModel, CategoryEntity>();
            CreateMap<CustomerModel, CustomerEntity>();
            CreateMap<ProductModel, ProductEntity>();
            CreateMap<SupplierModel, SupplierEntity>();
            CreateMap<UnitModel, UnitEntity>();
            CreateMap<OrderModel, OrderEntity>();
            CreateMap<OrderDetailModel, OrderDetailEntity>();
            CreateMap<OrderModel, SaleEntity>()
                .ForMember(dest => dest.SaleNumber, opts => opts.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.SaleDate, opts => opts.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.SaleDetails, opts => opts.MapFrom(src => src.OrderDetails));
            CreateMap<OrderDetailModel, SaleDetailEntity>();
        }
    }
}