using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Abstractions.Data;
using SmartStore.App.Models;
using SmartStore.App.Services.Data.Entities;

namespace SmartStore.App.Services.Business
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductModel>> GetListAsync()
        {
            var list = await _productRepository.Get();
            return _mapper.Map<IEnumerable<ProductEntity>, IEnumerable<ProductModel>>(list);
        }

        public async Task<IEnumerable<ProductModel>> GetFavoritesAsync()
        {
            var list = await _productRepository.Get(entity => entity.IsFavorite, entity => entity.UpdateAt);
            return _mapper.Map<IEnumerable<ProductEntity>, IEnumerable<ProductModel>>(list);
        }

        public async Task<ProductModel> SaveAsync(ProductModel model)
        {
            var entity = _mapper.Map<ProductModel, ProductEntity>(model);
            var result = await _productRepository.Upsert(entity);
            return result > 0 ? model : null;
        }

        public async Task<bool> DeleteAsync(ProductModel model)
        {
            var entity = _mapper.Map<ProductModel, ProductEntity>(model);
            var result = await _productRepository.Delete(entity);
            return result > 0;
        }
    }
}