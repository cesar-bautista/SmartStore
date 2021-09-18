using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SQLite;

namespace SmartStore.App.Abstractions.Data
{
    public interface IBaseRepository<T> where T : class, new()
    {
        Task<bool> CreateTable(bool alwaysCreate = false);
        Task<bool> DropTable();
        Task<DateTimeOffset> Sync(DateTimeOffset lastSync);
        Task<List<T>> Get();
        Task<T> Get(Guid id);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        Task<IEnumerable<T>> Get(int skip, int take);
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, int skip, int take);
        Task<List<T>> GetWithChildren(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        AsyncTableQuery<T> AsQueryable();
        Task<int> Insert(T entity);
        Task<int> Insert(IEnumerable<T> entities);
        Task InsertWithChildren(T entity);
        Task<int> Update(T entity);
        Task<int> Update(IEnumerable<T> entities);
        Task UpdateWithChildren(T entity);
        Task<int> Delete(T entity);
        Task<int> Upsert(T entity);
    }
}