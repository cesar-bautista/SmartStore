using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("SaleDetails")]
    public class SaleDetailEntity : BaseEntity
    {
        [ForeignKey(typeof(SaleEntity))]
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}