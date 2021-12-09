using System;
using SQLite;

namespace SmartStore.App.Services.Data.Entities
{
    public abstract class BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdateAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public bool IsDeleted { get; set; }
    }
}
