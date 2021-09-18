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

        public async Task<IEnumerable<CategoryModel>> GetListAsync(string filter = null)
        {
            var list = string.IsNullOrWhiteSpace(filter) ?
                await _categoryRepository.Get() :
                await _categoryRepository.Get(entity => entity.Name.ToLower().Contains(filter.ToLower())
                    || entity.Description.ToLower().Contains(filter.ToLower())
                    || entity.Code.ToLower().Contains(filter.ToLower()), entity => entity.Name);
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
            entity.IsDeleted = true;
            var result = await _categoryRepository.Update(entity);
            return result > 0;
        }
    }
}
