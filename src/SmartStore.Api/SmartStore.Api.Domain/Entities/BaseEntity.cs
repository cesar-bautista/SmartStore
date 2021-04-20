using System;

namespace SmartStore.Api.Domain.Entities
{
    public abstract class BaseEntity
    {
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}