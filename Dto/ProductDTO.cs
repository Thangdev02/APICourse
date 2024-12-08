namespace APICourse.Models
{
    public class ProductDto //Data Transfer Object
    {
        public int PId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
        public int? StockQuantity { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
