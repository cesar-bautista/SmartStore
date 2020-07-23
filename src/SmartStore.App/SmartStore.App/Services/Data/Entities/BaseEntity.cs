using System;
using SQLite;

namespace SmartStore.App.Services.Data.Entities
{
    public abstract class BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTimeOffset UpdateAt { get; set; }
        public bool Deleted { get; set; }
    }
}
