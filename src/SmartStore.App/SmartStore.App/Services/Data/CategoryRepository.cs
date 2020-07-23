using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(SQLiteAsyncConnection db) : base(db)
        {
        }
    }
}