using System;

namespace SmartStore.App.Models
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsReadOnly { get; set; }
    }
}