﻿using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public class OrderDetailRepository : BaseRepository<OrderDetailEntity>, IOrderDetailRepository
    {
        public OrderDetailRepository(SQLiteAsyncConnection db) : base(db)
        {
        }
    }
}