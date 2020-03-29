using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;

namespace SmartStore.App.Services
{
    public class SupplierService : ISupplierService
    {
        public async Task<IEnumerable<SupplierItemModel>> GetListAsync()
        {
            await Task.Delay(1000);

            var list = new List<SupplierItemModel>();
            for (var i = 1; i < 11; i++)
            {
                list.Add(new SupplierItemModel
                {
                    Id = i,
                    Code = $"00{i}",
                    Name = $"Supplier {i}",
                    Description = $"Supplier {i}",
                    Address = $"Address supplier {i}",
                    PhoneNumber = "0123456789",
                    Email = $"supplier{i}@server.com"
                });
            }

            return list;
        }
    }
}
