using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public class SaleRepository : BaseRepository<SaleEntity>, ISaleRepository
    {
        public SaleRepository(SQLiteAsyncConnection db) : base(db)
        {
        }
    }
}