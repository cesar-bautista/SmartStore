using System;

namespace SmartStore.App.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total => Quantity * Price;
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
    }
}
