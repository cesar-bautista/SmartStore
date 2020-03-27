namespace SmartStore.App.Models
{
    public class ProductItemModel
    {
        public int Id { get; set; }
        public bool IsLike { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Favorite { get; set; } = "favorite.png";
    }
}
