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
    public class SupplierService : ISupplierService
    {
        private readonly IMapper _mapper;
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(IMapper mapper, ISupplierRepository supplierRepository)
        {
            _mapper = mapper;
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<SupplierModel>> GetListAsync(string filter = null)
        {
            var list = string.IsNullOrWhiteSpace(filter) ?
                await _supplierRepository.Get() :
                await _supplierRepository.Get(entity => entity.Name.ToLower().Contains(filter.ToLower())
                    || entity.Description.ToLower().Contains(filter.ToLower())
                    || entity.Code.ToLower().Contains(filter.ToLower()), entity => entity.Name);
            return _mapper.Map<IEnumerable<SupplierEntity>, IEnumerable<SupplierModel>>(list);
        }

        public async Task<SupplierModel> SaveAsync(SupplierModel model)
        {
            var entity = _mapper.Map<SupplierModel, SupplierEntity>(model);
            var result = await _supplierRepository.Upsert(entity);
            return result > 0 ? model : null;
        }

        public async Task<bool> DeleteAsync(SupplierModel model)
        {
            var entity = _mapper.Map<SupplierModel, SupplierEntity>(model);
            var result = await _supplierRepository.Delete(entity);
            return result > 0;
        }
    }
}
