using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace SmartStore.App.Abstractions.Data
{
    public interface IBaseRepository<T> where T : class, new()
    {
        Task<bool> CreateTable();
        Task<DateTimeOffset> Sync(DateTimeOffset lastSync);
        Task<List<T>> Get();
        Task<T> Get(Guid id);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        AsyncTableQuery<T> AsQueryable();
        Task<int> Insert(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
        Task<int> Upsert(T entity);
    }
}