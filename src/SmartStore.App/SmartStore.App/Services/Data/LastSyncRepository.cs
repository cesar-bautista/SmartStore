using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public class LastSyncRepository : BaseRepository<LastSyncEntity>, ILastSyncRepository
    {
        public LastSyncRepository(SQLiteAsyncConnection db) : base(db)
        {
        }
    }
}