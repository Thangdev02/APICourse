using System;
using System.Collections.Generic;
namespace APICourse.Models;

public partial class Product
{
    public int PId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public string? ProductDescription { get; set; }

    public string? ProductCategory { get; set; }

    public int? StockQuantity { get; set; }

    public DateTime? DateAdded { get; set; }
}
