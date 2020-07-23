using System;
using SQLite;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("Products")]
    public class ProductEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFavorite { get; set; }
        public Guid UnitId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SupplierId { get; set; }
    }
}