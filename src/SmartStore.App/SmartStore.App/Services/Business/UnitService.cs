using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Models;

namespace SmartStore.App.Services.Business
{
    public class UnitService : IUnitService
    {
        public async Task<IEnumerable<UnitModel>> GetListAsync()
        {
            await Task.Delay(1000);

            var list = new List<UnitModel>();
            for (var i = 1; i < 11; i++)
            {
                list.Add(new UnitModel
                {
                    Id = i,
                    Name = $"Unit {i}",
                    Description = $"Description {i}"
                });
            }

            return list;
        }
    }
}
