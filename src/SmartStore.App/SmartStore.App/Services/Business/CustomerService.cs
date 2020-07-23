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
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IMapper mapper, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerModel>> GetListAsync()
        {
            var list = await _customerRepository.Get();
            return _mapper.Map<IEnumerable<CustomerEntity>, IEnumerable<CustomerModel>>(list);
        }

        public async Task<CustomerModel> SaveAsync(CustomerModel model)
        {
            var entity = _mapper.Map<CustomerModel, CustomerEntity>(model);
            var result = await _customerRepository.Upsert(entity);
            return result > 0 ? model : null;
        }

        public async Task<bool> DeleteAsync(CustomerModel model)
        {
            var entity = _mapper.Map<CustomerModel, CustomerEntity>(model);
            var result = await _customerRepository.Delete(entity);
            return result > 0;
        }
    }
}
