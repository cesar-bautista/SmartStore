﻿using System;
using SQLite;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("Customers")]
    public class CustomerEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Reference { get; set; }
        public int DiscountRate { get; set; }
        public int CreditRate { get; set; }
    }
}