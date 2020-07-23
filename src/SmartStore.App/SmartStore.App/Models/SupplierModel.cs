using System;

namespace SmartStore.App.Models
{
    public class SupplierModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public bool IsReadOnly { get; set; }
    }
}
