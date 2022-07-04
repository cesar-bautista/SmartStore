using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public class SaleDetailRepository : BaseRepository<SaleDetailEntity>, ISaleDetailRepository
    {
        public SaleDetailRepository(SQLiteAsyncConnection db) : base(db)
        {
        }
    }
}