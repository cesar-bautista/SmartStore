﻿using SQLite;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("Supplier")]
    public class SupplierEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}