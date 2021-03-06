﻿using AutoMapper;
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
        }
    }
}