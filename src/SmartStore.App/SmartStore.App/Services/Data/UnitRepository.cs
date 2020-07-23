using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public class UnitRepository : BaseRepository<UnitEntity>, IUnitRepository
    {
        public UnitRepository(SQLiteAsyncConnection db) : base(db)
        {
        }
    }
}