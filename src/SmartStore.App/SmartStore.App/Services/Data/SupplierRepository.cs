using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public class SupplierRepository : BaseRepository<SupplierEntity>, ISupplierRepository
    {
        public SupplierRepository(SQLiteAsyncConnection db) : base(db)
        {
        }
    }
}