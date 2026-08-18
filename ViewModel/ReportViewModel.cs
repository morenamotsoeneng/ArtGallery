using ArtGallery.Models;

public class ReportViewModel
{
    public decimal TotalRevenue { get; set; }

    public int TotalOrders { get; set; }

    public int TotalCustomers { get; set; }

    public int TotalArtworksSold { get; set; }

    public string? BestSellingArtwork { get; set; }

    public int BestSellingCount { get; set; }

    public List<Order> RecentOrders { get; set; } = new();
}
