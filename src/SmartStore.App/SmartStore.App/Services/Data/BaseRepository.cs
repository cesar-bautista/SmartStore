using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SmartStore.App.ViewModels.Base;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        private readonly SQLiteAsyncConnection _db;

        protected BaseRepository(SQLiteAsyncConnection db)
        {
            this._db = db;
        }

        public async Task<bool> CreateTable()
        {
            var result = await this._db.CreateTableAsync<T>();
            return result == CreateTableResult.Created || result == CreateTableResult.Migrated;
        }

        public async Task<DateTimeOffset> Sync(DateTimeOffset lastSync)
        {
            var restRepository = LocatorViewModel.Instance.Resolve<IRestRepository>();
            var changed = await Get(entity => entity.UpdateAt >= lastSync || entity.Deleted, entity => entity.UpdateAt);
            if (changed.Any())
                await restRepository.PostAsync<IEnumerable<T>>("/push", changed);

            foreach (var row in await restRepository.GetAsync<IEnumerable<T>>("/pull", lastSync))
            {
                if (await Get(row.Id) != null)
                    await Insert(row);
                else if (row.Deleted)
                    await Delete(row);
                else
                    await Update(row);
            }

            return DateTimeOffset.Now;
        }

        public AsyncTableQuery<T> AsQueryable() =>
            _db.Table<T>();

        public async Task<List<T>> Get() =>
            await _db.Table<T>().ToListAsync();

        public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = _db.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            return await query.ToListAsync();
        }

        public async Task<T> Get(Guid id) =>
            await _db.FindAsync<T>(id);

        public async Task<T> Get(Expression<Func<T, bool>> predicate) =>
            await _db.FindAsync<T>(predicate);

        public async Task<int> Insert(T entity)
        {
            entity.Id = Guid.NewGuid();
            entity.UpdateAt = DateTimeOffset.Now;
            return await _db.InsertAsync(entity);
        }

        public async Task<int> Update(T entity)
        {
            entity.UpdateAt = DateTimeOffset.Now;
            return await _db.UpdateAsync(entity);
        }

        public async Task<int> Delete(T entity)
        {
            entity.UpdateAt = DateTimeOffset.Now;
            entity.Deleted = true;
            return await _db.DeleteAsync(entity);
        }

        public async Task<int> Upsert(T entity)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            entity.UpdateAt = DateTimeOffset.Now;
            return await _db.InsertOrReplaceAsync(entity);
        }
    }
}