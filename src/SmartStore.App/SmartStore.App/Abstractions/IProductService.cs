﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Models;

namespace SmartStore.App.Abstractions
{
    public interface IProductService
    {
        Task<IEnumerable<ProductItemModel>> GetListAsync();
    }
}