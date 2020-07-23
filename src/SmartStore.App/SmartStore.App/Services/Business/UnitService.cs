using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Abstractions.Data;
using SmartStore.App.Models;
using SmartStore.App.Services.Data.Entities;

namespace SmartStore.App.Services.Business
{
    public class UnitService : IUnitService
    {
        private readonly IMapper _mapper;
        private readonly IUnitRepository _unitRepository;

        public UnitService(IMapper mapper, IUnitRepository unitRepository)
        {
            _mapper = mapper;
            _unitRepository = unitRepository;
        }

        public async Task<IEnumerable<UnitModel>> GetListAsync()
        {
            var list = await _unitRepository.Get();
            return _mapper.Map<IEnumerable<UnitEntity>, IEnumerable<UnitModel>>(list);
        }

        public async Task<UnitModel> SaveAsync(UnitModel model)
        {
            var entity = _mapper.Map<UnitModel, UnitEntity>(model);
            var result = await _unitRepository.Upsert(entity);
            return result > 0 ? model : null;
        }

        public async Task<bool> DeleteAsync(UnitModel model)
        {
            var entity = _mapper.Map<UnitModel, UnitEntity>(model);
            var result = await _unitRepository.Delete(entity);
            return result > 0;
        }
    }
}
