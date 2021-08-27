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
        protected readonly SQLiteAsyncConnection _db;

        protected BaseRepository(SQLiteAsyncConnection db)
        {
            this._db = db;
        }

        public virtual async Task<bool> CreateTable(bool alwaysCreate = false)
        {
            if (alwaysCreate)
            {
                var result = await this._db.CreateTableAsync<T>();
                return result == CreateTableResult.Created || result == CreateTableResult.Migrated;
            }
            else
            {
                string tableName = typeof(T).Name;
                var customAttributes = typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
                if (customAttributes.Count() > 0)
                {
                    tableName = (customAttributes.First() as TableAttribute).Name;
                }
                var tableInfo = await this._db.GetTableInfoAsync(tableName);
                if (tableInfo.Count == 0)
                    return await CreateTable(true);
                return false;
            }
        }

        public virtual async Task<bool> DropTable()
        {
            var result = await this._db.DropTableAsync<T>();
            return result > 0;
        }

        public virtual async Task<DateTimeOffset> Sync(DateTimeOffset lastSync)
        {
            var restRepository = LocatorViewModel.Instance.Resolve<IRestRepository>();
            var changed = await Get(entity => entity.UpdateAt >= lastSync || entity.Deleted, entity => entity.UpdateAt);
            if (changed.Any())
                await restRepository.PostAsync<IEnumerable<T>>("/push", changed);

            foreach (var row in await restRepository.GetAsync<IEnumerable<T>>("/pull", lastSync))
            {
                if (await Get(row.Id) == null)
                    await Insert(row);
                else if (row.Deleted)
                    await Delete(row);
                else
                    await Update(row);
            }

            return DateTimeOffset.Now;
        }

        public virtual AsyncTableQuery<T> AsQueryable() =>
            _db.Table<T>();

        public virtual async Task<List<T>> Get() =>
            await _db.Table<T>().ToListAsync();

        public virtual async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = _db.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            return await query.ToListAsync();
        }

        public virtual async Task<T> Get(Guid id) =>
            await _db.FindAsync<T>(id);

        public virtual async Task<T> Get(Expression<Func<T, bool>> predicate) =>
            await _db.FindAsync<T>(predicate);

        public virtual async Task<int> Insert(T entity)
        {
            entity.Id = Guid.NewGuid();
            entity.UpdateAt = DateTimeOffset.Now;
            return await _db.InsertAsync(entity);
        }

        public virtual async Task<int> Insert(IEnumerable<T> entities)
        {
            entities.All(c => { c.Id = Guid.NewGuid(); c.UpdateAt = DateTimeOffset.Now; return true; });
            return await _db.InsertAllAsync(entities);
        }

        public virtual async Task<int> Update(T entity)
        {
            entity.UpdateAt = DateTimeOffset.Now;
            return await _db.UpdateAsync(entity);
        }

        public virtual async Task<int> Update(IEnumerable<T> entities)
        {
            entities.All(c => { c.UpdateAt = DateTimeOffset.Now; return true; });
            return await _db.UpdateAllAsync(entities);
        }

        public virtual async Task<int> Delete(T entity)
        {
            entity.UpdateAt = DateTimeOffset.Now;
            entity.Deleted = true;
            return await _db.DeleteAsync(entity);
        }

        public virtual async Task<int> Upsert(T entity)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            entity.UpdateAt = DateTimeOffset.Now;
            return await _db.InsertOrReplaceAsync(entity);
        }
    }
}