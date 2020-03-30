﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;

namespace SmartStore.App.Services
{
    public class UnitService : IUnitService
    {
        public async Task<IEnumerable<UnitItemModel>> GetListAsync()
        {
            await Task.Delay(1000);

            var list = new List<UnitItemModel>();
            for (var i = 1; i < 11; i++)
            {
                list.Add(new UnitItemModel
                {
                    Id = i,
                    Name = $"Unit {i}",
                    Description = $"Unit {i}"
                });
            }

            return list;
        }
    }
}