using AutoMapper;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Abstractions.Data;
using SmartStore.App.Models;
using SmartStore.App.Services.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStore.App.Services.Business
{
    public class SaleService : ISaleService
    {
        private readonly IMapper _mapper;
        private readonly ISaleRepository _saleRepository;

        public SaleService(IMapper mapper, ISaleRepository saleRepository)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
        }

        public async Task<IEnumerable<OrderModel>> GetListAsync(string filter = null)
        {
            var list = string.IsNullOrWhiteSpace(filter) ?
                await _saleRepository.Get() :
                await _saleRepository.Get(entity => entity.SaleNumber.Contains(filter.ToLower()), entity => entity.SaleNumber);
            return _mapper.Map<IEnumerable<SaleEntity>, IEnumerable<OrderModel>>(list);
        }

        public async Task<OrderModel> GetListWithChildrenAsync(Guid filter)
        {
            var list = await _saleRepository.GetWithChildren(entity => entity.Id.Equals(filter));
            return list.Any() ? _mapper.Map<SaleEntity, OrderModel>(list.FirstOrDefault()) : new OrderModel();
        }

        public async Task SaveAsync(OrderModel model)
        {
            SaleEntity entity;
            if (string.IsNullOrWhiteSpace(model.OrderNumber))
            {
                entity = new SaleEntity
                {
                    CustomerId = model.CustomerId,
                    SaleDate = DateTimeOffset.Now,
                    SaleNumber = DateTimeOffset.Now.ToString("yyyyMMddHHmmss"),
                    TotalPrice = model.OrderDetails.Sum(m => m.Price * m.Quantity),
                    SaleDetails = _mapper.Map<IEnumerable<OrderDetailModel>, IEnumerable<SaleDetailEntity>>(model.OrderDetails).ToList()
                };
            }
            else
            {
                entity = _mapper.Map<OrderModel, SaleEntity>(model);
            }
            entity.SaleDetails.All(c => { c.SaleId = entity.Id; return true; });
            await _saleRepository.UpsertWithChildren(entity);
        }

        public async Task<bool> DeleteAsync(OrderModel model)
        {
            var entity = _mapper.Map<OrderModel, SaleEntity>(model);
            entity.IsDeleted = true;
            var result = await _saleRepository.Update(entity);
            return result > 0;
        }
    }
}