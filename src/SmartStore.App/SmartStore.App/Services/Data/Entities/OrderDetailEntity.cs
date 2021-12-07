using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("OrderDetails")]
    public class OrderDetailEntity : BaseEntity
    {
        [ForeignKey(typeof(OrderEntity))]
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        //public string UnitId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        //[ManyToOne(CascadeOperations = CascadeOperation.All)]
        //public ProductEntity Product { get; set; }
        //[ManyToOne(CascadeOperations = CascadeOperation.All)]
        //public UnitEntity Unit { get; set; }
        [ManyToOne]
        public OrderEntity Order { get; set; }
    }
}