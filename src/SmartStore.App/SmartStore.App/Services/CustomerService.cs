using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;

namespace SmartStore.App.Services
{
    public class CustomerService : ICustomerService
    {
        public async Task<IEnumerable<CustomerItemModel>> GetListAsync()
        {
            await Task.Delay(1000);

            var list = new List<CustomerItemModel>();
            for (var i = 1; i < 11; i++)
            {
                list.Add(new CustomerItemModel
                {
                    Id = i,
                    Code = $"00{i}",
                    Name = $"Customer {i}",
                    Description = $"Customer {i}",
                    Address = $"Address customer {i}",
                    PhoneNumber = "0123456789",
                    Email = $"customer{i}@server.com",
                    BirthDate = DateTime.Today,
                    DiscountRate = 10
                });
            }

            return list;
        }
    }
}
