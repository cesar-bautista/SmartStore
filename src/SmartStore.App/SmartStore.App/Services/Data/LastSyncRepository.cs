using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;
using System;
using System.Threading.Tasks;

namespace SmartStore.App.Services.Data
{
    public class LastSyncRepository : BaseRepository<LastSyncEntity>, ILastSyncRepository
    {
        public LastSyncRepository(SQLiteAsyncConnection db) : base(db)
        {
        }

        public override async Task<int> Upsert(LastSyncEntity entity)
        {
            entity.UpdateAt = DateTimeOffset.Now;
            return await _db.InsertOrReplaceAsync(entity);
        }
    }
}