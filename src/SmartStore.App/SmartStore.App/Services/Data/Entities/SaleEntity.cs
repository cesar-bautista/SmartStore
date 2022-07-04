using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("Sales")]
    public class SaleEntity : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string SaleNumber { get; set; }
        public DateTimeOffset SaleDate { get; set; }
        public double TotalPrice { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<SaleDetailEntity> SaleDetails { get; set; }
    }
}