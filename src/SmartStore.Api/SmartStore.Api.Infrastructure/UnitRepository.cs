using System.Data.SqlClient;
using SmartStore.Api.Domain.Entities;

namespace SmartStore.Api.Infrastructure
{
    public class UnitRepository : BaseRepository<SqlConnection, UnitEntity>
    {
        public UnitRepository(string connectionString)
            : base(connectionString)
        {
        }
    }
}