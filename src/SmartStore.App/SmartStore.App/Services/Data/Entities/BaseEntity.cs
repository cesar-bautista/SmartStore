using System;
using SQLite;

namespace SmartStore.App.Services.Data.Entities
{
    public abstract class BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdateAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
