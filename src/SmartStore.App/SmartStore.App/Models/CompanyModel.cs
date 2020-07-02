namespace SmartStore.App.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string Rfc { get; set; }
        public string CompanyName { get; set; }
        public string TradeName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string Logo { get; set; }
    }
}