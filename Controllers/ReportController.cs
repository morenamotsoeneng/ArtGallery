
using ArtGallery.Data;
using ArtGallery.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // REPORT DASHBOARD
        // ==========================
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            ViewBag.TotalCustomers = _context.Customers.Count();

            ViewBag.TotalOrders = _context.Orders.Count();

            ViewBag.TotalRevenue = _context.Orders
                .ToList()
                .Sum(o => o.TotalAmount);

            ViewBag.TotalArtworksSold = _context.OrderItems.Count();

            var bestArtwork = _context.OrderItems
                .Include(i => i.ArtWork)
                .AsEnumerable()
                .GroupBy(i => i.ArtWork?.Title)
                .Select(g => new
                {
                    Artwork = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            ViewBag.BestSellingArtwork = bestArtwork?.Artwork;
            ViewBag.BestSellingCount = bestArtwork?.Count ?? 0;

            ViewBag.RecentOrders = _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            return View();
        }

        // ==========================
        // SALES REPORT
        // ==========================
        public IActionResult Sales()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            var sales = _context.OrderItems
                .Include(o => o.ArtWork)
                .Include(o => o.Order)
                .ThenInclude(o => o.Customer)
                .ToList();

            return View(sales);
        }

        // ==========================
        // REVENUE REPORT
        // ==========================
        public IActionResult Revenue()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            var orders = _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
    }
}