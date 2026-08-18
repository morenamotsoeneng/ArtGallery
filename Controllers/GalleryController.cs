using ArtGallery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GalleryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Gallery Page
        public IActionResult Index(string searchString, int? categoryId)
        {
            // Customer must be logged in
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            var artworks = _context.ArtWorks
                .Include(a => a.Category)
                .Where(a => a.Status == true)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                artworks = artworks.Where(a =>
                    a.Title.Contains(searchString) ||
                    a.ArtistName.Contains(searchString));
            }

            // Filter by Category
            if (categoryId != null)
            {
                artworks = artworks.Where(a => a.CategoryId == categoryId);
            }

            ViewBag.Categories = _context.Categories.ToList();

            return View(artworks.ToList());
        }

        // Artwork Details
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            var artwork = _context.ArtWorks
                .Include(a => a.Category)
                .FirstOrDefault(a => a.ArtId == id);

            if (artwork == null)
                return NotFound();

            return View(artwork);
        }
    }
}
