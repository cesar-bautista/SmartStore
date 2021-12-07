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
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IMapper mapper, IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderModel>> GetListAsync(string filter = null)
        {
            //var list = string.IsNullOrWhiteSpace(filter) ?
            //    await _orderRepository.Get() :
            //    await _orderRepository.Get(entity => entity.OrderNumber.Contains(filter.ToLower()), entity => entity.OrderNumber);
            //return _mapper.Map<IEnumerable<OrderEntity>, IEnumerable<OrderModel>>(list);
            return new List<OrderModel>();
        }

        public async Task<OrderModel> SaveAsync(IEnumerable<OrderModel> model)
        {
            var entity = new OrderEntity
            {
                OrderDate = DateTimeOffset.Now,
                OrderNumber = DateTimeOffset.Now.ToString("yyyyMMddHHmmss"),
                TotalPrice = model.Sum(m => m.Price),
                OrderDetails = _mapper.Map<IEnumerable<OrderModel>, IEnumerable<OrderDetailEntity>>(model).ToList()
            };
            await _orderRepository.InsertWithChildren(entity);
            //return result > 0 ? model : null;
            return new OrderModel();
        }

        public async Task<bool> DeleteAsync(OrderModel model)
        {
            //var entity = _mapper.Map<OrderModel, OrderEntity>(model);
            //entity.IsDeleted = true;
            //var result = await _orderRepository.Update(entity);
            //return result > 0;
            return true;
        }
    }
}