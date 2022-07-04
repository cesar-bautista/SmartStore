using System;
using System.Collections.Generic;

namespace SmartStore.App.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderDetailModel> OrderDetails { get; set; }
    }
}
