using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;
using SQLite;

namespace SmartStore.App.Services.Data
{
    public class CustomerRepository : BaseRepository<CustomerEntity>, ICustomerRepository
    {
        public CustomerRepository(SQLiteAsyncConnection db) : base(db)
        {
        }
    }
}