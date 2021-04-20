using System;

namespace SmartStore.Api.Domain.Entities
{
    public class UnitEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}