﻿using SQLite;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("Categories")]
    public class CategoryEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}