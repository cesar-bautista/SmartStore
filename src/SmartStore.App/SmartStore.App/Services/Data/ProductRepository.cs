using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        public ProductRepository(SQLiteAsyncConnection db) : base(db)
        {
        }
    }
}