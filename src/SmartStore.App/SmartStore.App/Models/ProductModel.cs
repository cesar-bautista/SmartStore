namespace SmartStore.App.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFavorite { get; set; }
        public int UnitId { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
    }
}