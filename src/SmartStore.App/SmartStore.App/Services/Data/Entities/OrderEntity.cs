using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("Orders")]
    public class OrderEntity : BaseEntity
    {
        public string OrderNumber { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public double TotalPrice { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<OrderDetailEntity> OrderDetails { get; set; }
    }
}