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
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryModel>> GetListAsync()
        {
            var list = await _categoryRepository.Get();
            return _mapper.Map<IEnumerable<CategoryEntity>, IEnumerable<CategoryModel>>(list);
        }

        public async Task<CategoryModel> SaveAsync(CategoryModel model)
        {
            var entity = _mapper.Map<CategoryModel, CategoryEntity>(model);
            var result = await _categoryRepository.Upsert(entity);
            return result > 0 ? model : null;
        }

        public async Task<bool> DeleteAsync(CategoryModel model)
        {
            var entity = _mapper.Map<CategoryModel, CategoryEntity>(model);
            var result = await _categoryRepository.Delete(entity);
            return result > 0;
        }
    }
}
