using System;
using SQLite;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("LastSync")]
    public class LastSyncEntity : BaseEntity
    {
        public DateTimeOffset LastSync { get; set; }

        public LastSyncEntity()
        {
            Id = Guid.Empty;
            LastSync = DateTimeOffset.MinValue;
        }
    }
}
